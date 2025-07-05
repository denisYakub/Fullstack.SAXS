using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Contracts
{
    public abstract class ParticleFactory
    {
        public abstract ParticleTypes Type { get; }
        public abstract IEnumerable<Particle> GetInfParticles(
            double minSize, double maxSize, double sizeShape, double sizeScale,
            double alphaRotation, double betaRotation, double gammaRotation,
            double minX, double maxX,
            double minY, double maxY,
            double minZ, double maxZ
        );
    }
}
