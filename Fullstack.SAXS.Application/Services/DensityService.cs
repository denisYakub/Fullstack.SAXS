using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Extensions;
using Fullstack.SAXS.Domain.Models;
using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Application.Services
{
    public class DensityService : IDensityService
    {
        public async Task<(double[] x, double[] y)> CreatePhiCoordAsync(
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
                var (segmentPoint, segmentParticlePoints) = await segmentTasks[i].ConfigureAwait(false);

                double phi = segmentParticlePoints / (double)segmentPoint;

                phiValues[i].value = phi;
            }

            var result = phiValues.Select((phi, index) => (index, phi.value)).ToList();

            return (
                result.Select(x => (double)x.index).ToArray(),
                result.Select(x => x.value).ToArray()
            );
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
                var distance = Vector3D.Distance(new(0, 0, 0), point);

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
    }
}
