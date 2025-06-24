using System.Numerics;
using System.Text.Json;
using Fullstack.SAXS.Domain.Commands;
using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Octrees;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Entities.Regions;

namespace Fullstack.SAXS.Application
{
    public class SysService(AreaParticleFactory factory, IStorage storage, IGraphService graph)
    {
        public void Create(
            string? userId,
            float AreaSize, int AreaNumber, int ParticleNumber,
            float ParticleMinSize, float ParticleMaxSize,
            float ParticleSizeShape, float ParticleSizeScale,
            float ParticleAlphaRotation,
            float ParticleBetaRotation,
            float ParticleGammaRotation
        )
        {
            try
            {
                var idUser = Guid.Parse(userId);

                var startRegion = new Region(new(0, 0, 0), AreaSize * 2);
                var octree = new Octree(startRegion.MaxDepth(3 * ParticleMaxSize), startRegion);

                var areas = factory.GetAreas(AreaSize, AreaNumber, ParticleMaxSize);

                Parallel.ForEach(areas, area => {
                    var infParticles =
                        factory
                        .GetInfParticles(
                            ParticleMinSize, ParticleMaxSize,
                            ParticleSizeShape, ParticleSizeScale,
                            ParticleAlphaRotation,
                            ParticleBetaRotation,
                            ParticleGammaRotation,
                            -AreaSize, AreaSize,
                            -AreaSize, AreaSize,
                            -AreaSize, AreaSize
                        );

                    area.Fill(infParticles, ParticleNumber);

                    storage.AddAsync(area);
                });

                storage.SaveAsync(idUser);
            }
            catch (ArgumentNullException)
            {
                throw new UnauthorizedAccessException("userId is null");
            }
            catch (FormatException)
            {
                throw new UnauthorizedAccessException("userId is not Guid");
            }
        }

        public string Get(Guid id)
        {
            var area = storage.GetArea(id);

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = true,
            };

            var json = JsonSerializer.Serialize(area, options);

            return json;
        }

        public async Task<string> CreateIntensOptGrafAsync(
            Guid id,
            float QMin, float QMax, int QNum
        )
        {
            var area = storage.GetArea(id);
            var qs = CreateQs(QMin, QMax, QNum);

            var (x, y) = CreateIntensOptCoord(area, in qs);

            return await graph.GetHtmlPageAsync(x, y);
        }

        public async Task<string> CreatePhiGrafAsync(Guid id, int layersNum)
        {
            var area = await storage.GetAreaAsync(id);

            var (x, y) = await CreatePhiCoordAsync(area, layersNum);

            return await graph.GetHtmlPageAsync(x, y);
        }

        private static async Task<(float[] x, float[] y)> CreatePhiCoordAsync(
            Area area, int numberOfLayers = 5, int numberOfPoints = 100_000
        )
        {
            var segmentsR = CreateSegmentsRadii(area.OuterRadius, numberOfLayers);
            var points = CreateRandomPoints(area.OuterRadius, numberOfPoints);

            var segmentTasks =
                new Task<(int segmentPoint, int segmentParticlePoints)>[numberOfLayers];

            for (int i = 0; i < segmentTasks.Length; i++)
            {
                var radiusMin = segmentsR[i];
                var radiusMax = segmentsR[i + 1];

                segmentTasks[i] =
                    new Task<(int segmentPoint, int segmentParticlePoints)>(
                        () => CountHits(radiusMin, radiusMax, in points, area.Particles)
                    );
                segmentTasks[i].Start();
            }

            var phiValues = new (float value, bool isSet)[segmentTasks.Length];

            for (int i = 0; i < segmentTasks.Length; i++)
            {
                var (segmentPoint, segmentParticlePoints) = await segmentTasks[i];

                float phi = segmentParticlePoints / (float)segmentPoint;

                phiValues[i].value = phi;
            }

            var result = phiValues.Select((phi, index) => (index, phi.value));

            return (
                result.Select(x => (float)x.index).ToArray(),
                result.Select(x => x.value).ToArray()
            );
        }

        private static (float[] x, float[] y) CreateIntensOptCoord(
            Area area, in float[] qs
        )
        {
            var (localVolume, globalVolume, tmpConst) = Eval(area, qs);
            var nC = area.Particles.Count();

            var (q, I) = IntenceOpt(qs, globalVolume, tmpConst, localVolume, area);

            return (q.ToArray(), I.ToArray());
        }

        private static float[] CreateQs(float QMin, float QMax, int QNum)
        {
            var random = new Random();
            var qs = new float[QNum];

            for (var i = 0; i < QNum; i++)
                qs[i] = random.GetEvenlyRandom(QMin, QMax);

            return qs;
        }

        private static float[] CreateSegmentsRadii(float areaOuterR, int radiiNum)
        {
            var radii = new float[radiiNum + 1];

            for (int i = 0; i < radiiNum; i++)
            {
                radii[i] = (float)Math.Pow(i * (Math.Pow(areaOuterR, 3) / radiiNum), 1.0f / 3);
            }

            radii[radiiNum] = areaOuterR;

            return radii;
        }

        private static Vector3[] CreateRandomPoints(float edge, int pointsNum)
        {
            var random = new Random();
            var points = new Vector3[pointsNum];

            for (int i = 0; i < pointsNum; i++)
            {
                points[i] = new Vector3(
                    random.GetEvenlyRandom(-edge, edge),
                    random.GetEvenlyRandom(-edge, edge),
                    random.GetEvenlyRandom(-edge, edge)
                );
            }

            return points;
        }

        private static (int segmentPoint, int segmentParticlePoints) CountHits(
            float radiusMin, float radiusMax,
            in Vector3[] points,
            IEnumerable<Particle> particles
        )
        {
            int segmentPoint = 0;
            int segmentParticlePoints = 0;

            foreach (var point in points)
            {
                var distance = Vector3.Distance(new (0, 0, 0), point);

                if (distance >= radiusMin && distance <= radiusMax)
                {
                    segmentPoint++;

                    foreach (var particle in particles)
                        if (particle.Contains(point))
                            segmentParticlePoints++;
                }
            }

            return (segmentPoint, segmentParticlePoints);
        }

        private static (
            IEnumerable<float> localVolume,
            IEnumerable<float> globalVolume,
            IEnumerable<float> tmpConst
        ) Eval(Area area, in float[] qs)
        {
            var vConst = 4 / 3 * MathF.PI;

            var outerSphereVolume = vConst * MathF.Pow(area.OuterRadius, 3);

            var localVolume = area.Particles
                .Select(fullerene => vConst * MathF.Pow(fullerene.OuterSphereRadius, 3));

            var globalVolume = qs.Select(qI => outerSphereVolume * Factor(area.OuterRadius * qI));

            var tmpConst = area.Particles
                .Select(fullerene => fullerene.Center.Length());

            return (localVolume, globalVolume, tmpConst);
        }

        private static float Factor(float x)
            => 3 * (MathF.Sin(x) - x * MathF.Cos(x)) / MathF.Pow(x, 3);

        private static (IEnumerable<float> q, IEnumerable<float> I) IntenceOpt(
            IEnumerable<float> qs,
            IEnumerable<float> globalVolume,
            IEnumerable<float> tmpConst,
            IEnumerable<float> localVolume,
            Area area)
        {
            int qNum = qs.Count();
            int fullereneNum = area.Particles.Count();

            Span<float> localVolumeSqr = localVolume.Select(v => MathF.Pow(v, 2)).ToArray();

            var qR = new FloatMatrix(new float[qNum * fullereneNum], fullereneNum);
            var factorConst = new FloatMatrix(new float[qNum * fullereneNum], fullereneNum);
            var factorSqr = new FloatMatrix(new float[qNum * fullereneNum], fullereneNum);

            for (int i = 0; i < qNum; i++)
                for (int j = 0; j < fullereneNum; j++)
                {
                    qR[i, j] = qs.ElementAt(i) * area.Particles.ElementAt(j).OuterSphereRadius;
                    factorConst[i, j] = Factor(qR[i, j]);
                    factorSqr[i, j] = factorConst[i, j] * factorConst[i, j];
                }

            Span<float> s2 = new float[qNum];
            Span<float> spFirstSummand = new float[qNum];

            for (int i = 0; i < qNum; i++)
                for (int j = 0; j < fullereneNum; j++)
                {
                    s2[i] += factorSqr[i, j] * localVolumeSqr[j];
                    spFirstSummand[i] += localVolume.ElementAt(j) * factorConst[i, j] * Sinc(qs.ElementAt(i) * tmpConst.ElementAt(j));
                }

            Span<float> spFactors = new float[qNum];

            for (int i = 0; i < fullereneNum; i++)
                for (int j = i + 1; j < fullereneNum; j++)
                {
                    var dist = Vector3.Distance(area.Particles.ElementAt(i).Center, area.Particles.ElementAt(j).Center);
                    var vol = localVolume.ElementAt(j) * localVolume.ElementAt(i);

                    for (int k = 0; k < qNum; k++)
                        spFactors[k] += factorConst[k, i] * factorConst[k, j] * Sinc(qs.ElementAt(k) * dist) * vol;
                }

            Span<float> spGlobalArray = globalVolume.ToArray();

            var I = new float[qNum];

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
            private readonly Span<float> _data;
            private readonly int _cols;

            public FloatMatrix(Span<float> data, int cols)
            {
                _data = data;
                _cols = cols;
            }

            public ref float this[int row, int col]
                => ref _data[row * _cols + col];

            public int Rows => _data.Length / _cols;
            public int Columns => _cols;
        }

        private static float Sinc(float x)
            => MathF.Sin(x) / x;
    }
}
