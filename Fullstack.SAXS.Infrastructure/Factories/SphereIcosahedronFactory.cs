using Fullstack.SAXS.Server.Domain.Commands;
using Fullstack.SAXS.Server.Domain.Entities.Areas;
using Fullstack.SAXS.Server.Domain.Entities.Octrees;
using Fullstack.SAXS.Server.Domain.Entities.Particles;
using MathNet.Numerics.Distributions;

namespace Fullstack.SAXS.Server.Infastructure.Factories
{
    public class SphereIcosahedronFactory : AreaParticleFactory
    {
        public override IEnumerable<Area> GetAreas(float areaSize, int number, float maxParticleSize)
        {
            for (int i = 0; i < number; i++) 
                yield return new SphereArea(i, areaSize, maxParticleSize);
        }

        public override IEnumerable<Particle> GetInfParticles(
            float minSize, float maxSize, 
            float sizeShape, float sizeScale, 
            float alphaRotation, float betaRotation, float gammaRotation,
            float minX, float maxX,
            float minY, float maxY,
            float minZ, float maxZ
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
