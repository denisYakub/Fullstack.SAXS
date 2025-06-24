using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Contracts
{
    public abstract class AreaParticleFactory
    {
        public abstract IEnumerable<Area> GetAreas(float areaSize, int number, float maxParticleSize);
        public abstract IEnumerable<Particle> GetInfParticles(
            float minSize, float maxSize, float sizeShape, float sizeScale,
            float alphaRotation, float betaRotation, float gammaRotation,
            float minX, float maxX,
            float minY, float maxY,
            float minZ, float maxZ
        );
    }
}
