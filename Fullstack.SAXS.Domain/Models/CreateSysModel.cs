using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Models
{
    public record CreateSysModel(
        Guid UserId,
        double AreaSize, int AreaNumber, int ParticleNumber, ParticleTypes ParticleType,
        double ParticleMinSize, double ParticleMaxSize,
        double ParticleSizeShape, double ParticleSizeScale,
        double ParticleAlphaRotation,
        double ParticleBetaRotation,
        double ParticleGammaRotation
    )
    {
        public CreateParticelModel CreateParticelModel
        {
            get
            {
                return new CreateParticelModel(
                    ParticleMinSize, ParticleMaxSize,
                    ParticleSizeShape, ParticleSizeScale,
                    ParticleAlphaRotation, ParticleBetaRotation, ParticleGammaRotation,
                    -AreaSize, AreaSize,
                    -AreaSize, AreaSize,
                    -AreaSize, AreaSize
                );
            }
        }
    }
}
