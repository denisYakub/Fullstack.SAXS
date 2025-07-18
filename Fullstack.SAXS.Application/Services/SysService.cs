﻿using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.Extensions;
using Fullstack.SAXS.Domain.Models;
using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Application.Services
{
    public class SysService(AreaFactory areaF, IParticleFactoryResolver prtclFResolver, IStorage storage, IGraphService graph) : ISysService
    {
        public async Task CreateAsync(Guid userId, CreateSysData data)
        {
            var areas = areaF.GetAreas(data.AreaSize, data.AreaNumber, data.ParticleMaxSize).ToArray();

            Parallel.For(0, areas.Length, i => {
                var infParticles = 
                    prtclFResolver
                    .Resolve(data.particleType)
                    .GetInfParticles(
                        data.ParticleMinSize, data.ParticleMaxSize, 
                        data.ParticleSizeShape, data.ParticleSizeScale, 
                        data.ParticleAlphaRotation, data.ParticleBetaRotation, data.ParticleGammaRotation,
                        -data.AreaSize, data.AreaSize,
                        -data.AreaSize, data.AreaSize,
                        -data.AreaSize, data.AreaSize);

                areas[i].Fill(infParticles, data.ParticleNumber);
            });

            await storage.AddRangeAsync(areas, userId);
        }

        public async Task<string> CreateIntensOptGraphAsync(Guid id, CreateQIData data)
        {
            var area = await storage.GetAreaAsync(id);
            var qs = CreateQs(data.QMin, data.QMax, data.QNum, data.StepType);

            var (x, y) = CreateIntensOptCoord(area, in qs);

            return await graph.GetHtmlPageAsync(x, y);
        }

        public async Task<string> CreatePhiGraphAsync(Guid userId, Guid id, int layersNum)
        {
            var area = await storage.GetAreaAsync(id);

            var (x, y) = await CreatePhiCoordAsync(area, layersNum);

            await storage.SaveAvgPhiAsync(userId, id, y.Average());

            return await graph.GetHtmlPageAsync(x, y);
        }

        public static double[] CreateQs(double QMin, double QMax, int QNum, StepTypes StepType = StepTypes.Linear)
        {
            double[] result = new double[QNum];

            switch (StepType)
            {
                case StepTypes.Linear:
                    for (int i = 0; i < QNum; i++)
                        result[i] = QMin + i * ((QMax - QMin) / QNum - 1);
                    break;
                case StepTypes.Logarithmic:
                    for (int i = 0; i < QNum; i++)
                    {
                        var logMin = Math.Log(QMin);
                        var logMax = Math.Log(QMax);
                        var t = (double)i / (QNum - 1);
                        result[i] = Math.Exp(logMin + t * (logMax - logMin));
                    }
                    break;
            }

            return result;
        }

        private static async Task<(double[] x, double[] y)> CreatePhiCoordAsync(
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

            var phiValues = new (double value, bool isSet)[segmentTasks.Length];

            for (int i = 0; i < segmentTasks.Length; i++)
            {
                var (segmentPoint, segmentParticlePoints) = await segmentTasks[i];

                double phi = segmentParticlePoints / (double)segmentPoint;

                phiValues[i].value = phi;
            }

            var result = phiValues.Select((phi, index) => (index, phi.value));

            return (
                result.Select(x => (double)x.index).ToArray(),
                result.Select(x => x.value).ToArray()
            );
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

        private static double[] CreateSegmentsRadii(double areaOuterR, int radiiNum)
        {
            var radii = new double[radiiNum + 1];

            for (int i = 0; i < radiiNum; i++)
            {
                radii[i] = Math.Pow(i * (Math.Pow(areaOuterR, 3) / radiiNum), 1.0f / 3);
            }

            radii[radiiNum] = areaOuterR;

            return radii;
        }

        private static Vector3D[] CreateRandomPoints(double edge, int pointsNum)
        {
            var random = new Random();
            var points = new Vector3D[pointsNum];

            for (int i = 0; i < pointsNum; i++)
            {
                points[i] = new Vector3D(
                    random.GetEvenlyRandom(-edge, edge),
                    random.GetEvenlyRandom(-edge, edge),
                    random.GetEvenlyRandom(-edge, edge)
                );
            }

            return points;
        }

        private static (int segmentPoint, int segmentParticlePoints) CountHits(
            double radiusMin, double radiusMax,
            in Vector3D[] points,
            IEnumerable<Particle> particles
        )
        {
            int segmentPoint = 0;
            int segmentParticlePoints = 0;

            foreach (var point in points)
            {
                var distance = Vector3D.Distance(new (0, 0, 0), point);

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
            int qNum = qs.Count();
            int fullereneNum = area.Particles.Count();

            Span<double> localVolumeSqr = localVolume.Select(v => Math.Pow(v, 2)).ToArray();

            var qR = new FloatMatrix(new double[qNum * fullereneNum], fullereneNum);
            var factorConst = new FloatMatrix(new double[qNum * fullereneNum], fullereneNum);
            var factorSqr = new FloatMatrix(new double[qNum * fullereneNum], fullereneNum);

            for (int i = 0; i < qNum; i++)
                for (int j = 0; j < fullereneNum; j++)
                {
                    qR[i, j] = qs.ElementAt(i) * area.Particles.ElementAt(j).OuterSphereRadius;
                    factorConst[i, j] = Factor(qR[i, j]);
                    factorSqr[i, j] = factorConst[i, j] * factorConst[i, j];
                }

            Span<double> s2 = new double[qNum];
            Span<double> spFirstSummand = new double[qNum];

            for (int i = 0; i < qNum; i++)
                for (int j = 0; j < fullereneNum; j++)
                {
                    s2[i] += factorSqr[i, j] * localVolumeSqr[j];
                    spFirstSummand[i] += localVolume.ElementAt(j) * factorConst[i, j] * Sinc(qs.ElementAt(i) * tmpConst.ElementAt(j));
                }

            Span<double> spFactors = new double[qNum];

            for (int i = 0; i < fullereneNum; i++)
                for (int j = i + 1; j < fullereneNum; j++)
                {
                    var dist = Vector3D.Distance(area.Particles.ElementAt(i).Center, area.Particles.ElementAt(j).Center);
                    var vol = localVolume.ElementAt(j) * localVolume.ElementAt(i);

                    for (int k = 0; k < qNum; k++)
                        spFactors[k] += factorConst[k, i] * factorConst[k, j] * Sinc(qs.ElementAt(k) * dist) * vol;
                }

            Span<double> spGlobalArray = globalVolume.ToArray();

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
