using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Models
{
    public record CreateSysData(
        double AreaSize, int AreaNumber, int ParticleNumber, ParticleTypes ParticleType,
        double ParticleMinSize, double ParticleMaxSize,
        double ParticleSizeShape, double ParticleSizeScale,
        double ParticleAlphaRotation,
        double ParticleBetaRotation,
        double ParticleGammaRotation
    );
}
