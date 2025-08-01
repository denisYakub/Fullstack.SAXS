using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Application.Services
{
    public class IntenceService(IStorage storage, IGraphService graph) : IIntenceService
    {
        public async Task<string> CreateIntensOptGraphAsync(IntensityCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Shouldn't be null.");

            var area = await storage
                .GetAreaAsync(dto.AreaId)
                .ConfigureAwait(false);

            var qs = CreateQVector(dto.QMin, dto.QMax, dto.QNum, dto.StepType);

            var (x, y) = CreateIntensOptCoord(area, in qs);

            return await graph
                .GetHtmlPageAsync(x, y, "Empty", "Empty", "Empty")
                .ConfigureAwait(false);
        }

        public static double[] CreateQVector(double QMin, double QMax, int QNum, StepTypes StepType = StepTypes.Linear)
        {
            double[] result = new double[QNum];

            switch (StepType)
            {
                case StepTypes.Linear:
                    var lineStep = (QMax - QMin) / (QNum - 1);

                    for (int i = 0; i < QNum; i++)
                        result[i] = QMin + i * lineStep;

                    break;
                case StepTypes.Logarithmic:
                    double baseValue = 10.0d;

                    var logMin = Math.Log10(QMin);
                    var logMax = Math.Log10(QMax);

                    var logStep = (logMax - logMin) / (QNum - 1);

                    for (int i = 0; i < QNum; i++)
                        result[i] = logMin + i * logStep;

                    result = [.. result.Select(v => Math.Pow(baseValue, v))];

                    break;
                case StepTypes.Geometric:

                    break;
            }

            return result;
        }

        private static (double[] x, double[] y) CreateIntensOptCoord(
            Area area, in double[] qs
        )
        {
            var (localVolume, globalVolume, tmpConst) = Eval(area, qs);
            var nC = area.Particles.Count();

            var (q, I) = IntenceOpt(qs, globalVolume, tmpConst, localVolume, area);

            return (q.ToArray(), I.ToArray());
        }

        private static (
            IEnumerable<double> localVolume,
            IEnumerable<double> globalVolume,
            IEnumerable<double> tmpConst
        ) Eval(Area area, in double[] qs)
        {
            var vConst = 4 / 3 * Math.PI;

            var outerSphereVolume = vConst * Math.Pow(area.OuterRadius, 3);

            var localVolume = area.Particles
                .Select(fullerene => vConst * Math.Pow(fullerene.OuterSphereRadius, 3));

            var globalVolume = qs.Select(qI => outerSphereVolume * Factor(area.OuterRadius * qI));

            var tmpConst = area.Particles
                .Select(fullerene => fullerene.Center.Length());

            return (localVolume, globalVolume, tmpConst);
        }

        private static double Factor(double x)
            => 3 * (Math.Sin(x) - x * Math.Cos(x)) / Math.Pow(x, 3);

        private static (IEnumerable<double> q, IEnumerable<double> I) IntenceOpt(
            IEnumerable<double> qs,
            IEnumerable<double> globalVolume,
            IEnumerable<double> tmpConst,
            IEnumerable<double> localVolume,
            Area area)
        {
            if (qs == null)
                throw new ArgumentNullException(nameof(qs), "Shouldn't be null.");

            if (globalVolume == null)
                throw new ArgumentNullException(nameof(globalVolume), "Shouldn't be null.");

            if (tmpConst == null)
                throw new ArgumentNullException(nameof(tmpConst), "Shouldn't be null.");

            if (localVolume == null)
                throw new ArgumentNullException(nameof(localVolume), "Shouldn't be null.");

            var qsArr = qs.ToArray();
            var localVolumeArr = localVolume.ToArray();
            var tmpConstArr = tmpConst.ToArray();
            var globalVolumeArr = globalVolume.ToArray();
            var areaParticlesArr = area.Particles.ToArray();

            int qNum = qsArr.Length;
            int fullereneNum = areaParticlesArr.Length;

            Span<double> localVolumeSqr = localVolumeArr.Select(v => Math.Pow(v, 2)).ToArray();

            var qR = new FloatMatrix(new double[qNum * fullereneNum], fullereneNum);
            var factorConst = new FloatMatrix(new double[qNum * fullereneNum], fullereneNum);
            var factorSqr = new FloatMatrix(new double[qNum * fullereneNum], fullereneNum);

            for (int i = 0; i < qNum; i++)
                for (int j = 0; j < fullereneNum; j++)
                {
                    qR[i, j] = qsArr[i] * areaParticlesArr[j].OuterSphereRadius;
                    factorConst[i, j] = Factor(qR[i, j]);
                    factorSqr[i, j] = factorConst[i, j] * factorConst[i, j];
                }

            Span<double> s2 = new double[qNum];
            Span<double> spFirstSummand = new double[qNum];

            for (int i = 0; i < qNum; i++)
                for (int j = 0; j < fullereneNum; j++)
                {
                    s2[i] += factorSqr[i, j] * localVolumeSqr[j];
                    spFirstSummand[i] += localVolumeArr[j] * factorConst[i, j] * Sinc(qsArr[i] * tmpConstArr[j]);
                }

            Span<double> spFactors = new double[qNum];

            for (int i = 0; i < fullereneNum; i++)
                for (int j = i + 1; j < fullereneNum; j++)
                {
                    var dist = Vector3D.Distance(areaParticlesArr[i].Center, areaParticlesArr[j].Center);
                    var vol = localVolumeArr[j] * localVolumeArr[i];

                    for (int k = 0; k < qNum; k++)
                        spFactors[k] += factorConst[k, i] * factorConst[k, j] * Sinc(qsArr[k] * dist) * vol;
                }

            Span<double> spGlobalArray = globalVolumeArr;

            var I = new double[qNum];

            for (int k = 0; k < qNum; k++)
            {
                var spG = spGlobalArray[k];
                I[k] = s2[k]
                     + 2 * spFactors[k]
                     + fullereneNum * fullereneNum * spG * spG
                     - 2 * fullereneNum * spFirstSummand[k] * spG;
            }

            return (qs, I);
        }

        private ref struct FloatMatrix
        {
            private readonly Span<double> _data;
            private readonly int _cols;

            public FloatMatrix(Span<double> data, int cols)
            {
                _data = data;
                _cols = cols;
            }

            public ref double this[int row, int col]
                => ref _data[row * _cols + col];

            public int Rows => _data.Length / _cols;
            public int Columns => _cols;
        }

        private static double Sinc(double x)
            => Math.Sin(x) / x;
    }
}
