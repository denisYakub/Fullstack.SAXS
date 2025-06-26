using Fullstack.SAXS.Domain.Commands;
using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;
using MathNet.Numerics.Distributions;

namespace Fullstack.SAXS.Infrastructure.Factories
{
    public class SphereIcosahedronFactory : AreaParticleFactory
    {
        public override IEnumerable<Area> GetAreas(double areaSize, int number, double maxParticleSize)
        {
            for (int i = 0; i < number; i++) 
                yield return new SphereArea(i, areaSize, maxParticleSize);
        }

        public override IEnumerable<Particle> GetInfParticles(
            double minSize, double maxSize,
            double sizeShape, double sizeScale,
            double alphaRotation, double betaRotation, double gammaRotation,
            double minX, double maxX,
            double minY, double maxY,
            double minZ, double maxZ
        )
        {
            var random = new Random();
            var gamma = new Gamma(sizeShape, sizeScale);

            while (true)
            {
                var size = gamma.GetGammaRandom(minSize, maxSize);

                var a = random.GetEvenlyRandom(-alphaRotation, alphaRotation);
                var b = random.GetEvenlyRandom(-betaRotation, betaRotation);
                var g = random.GetEvenlyRandom(-gammaRotation, gammaRotation);

                var x = random.GetEvenlyRandom(minX, maxX);
                var y = random.GetEvenlyRandom(minY, maxY);
                var z = random.GetEvenlyRandom(minZ, maxZ);

                yield return new IcosahedronParticle(size, new (x, y, z), new (a, b, g));
            }
        }
    }
}
