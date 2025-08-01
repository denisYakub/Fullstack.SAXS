using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Dtos
{
    public record SystemCreateDto(
        Guid UserId,
        double AreaSize, 
        int AreaNumber, int ParticleNumber, 
        AreaTypes AreaType, ParticleTypes ParticleType,
        double ParticleMinSize, double ParticleMaxSize,
        double ParticleSizeShape, double ParticleSizeScale,
        double ParticleAlphaRotation,
        double ParticleBetaRotation,
        double ParticleGammaRotation
    )
    {
        public ParticleCreateDTO ParticleCreateDTO
        {
            get
            {
                return new ParticleCreateDTO(
                    ParticleNumber, ParticleType,
                    ParticleMinSize, ParticleMaxSize,
                    ParticleSizeShape, ParticleSizeScale,
                    ParticleAlphaRotation, ParticleBetaRotation, ParticleGammaRotation,
                    -AreaSize, AreaSize,
                    -AreaSize, AreaSize,
                    -AreaSize, AreaSize
                );
            }
        }

        public AreaCreateDTO AreaCreateDTO
        {
            get
            {
                return new AreaCreateDTO(
                    AreaSize, AreaNumber, AreaType
                );
            }
        }
    }
}
