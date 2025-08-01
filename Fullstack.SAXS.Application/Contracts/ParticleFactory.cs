using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Application.Contracts
{
    public abstract class ParticleFactory
    {
        public abstract ParticleTypes Type { get; }
        public abstract IEnumerable<Particle> GetParticlesInf(ParticleCreateDTO dto);
    }
}
