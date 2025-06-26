using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Contracts
{
    public abstract class AreaParticleFactory
    {
        public abstract IEnumerable<Area> GetAreas(double areaSize, int number, double maxParticleSize);
        public abstract IEnumerable<Particle> GetInfParticles(
            double minSize, double maxSize, double sizeShape, double sizeScale,
            double alphaRotation, double betaRotation, double gammaRotation,
            double minX, double maxX,
            double minY, double maxY,
            double minZ, double maxZ
        );
    }
}
