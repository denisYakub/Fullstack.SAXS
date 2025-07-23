using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.Models;

namespace Fullstack.SAXS.Domain.Contracts
{
    public abstract class ParticleFactory
    {
        public abstract ParticleTypes Type { get; }
        public abstract IEnumerable<Particle> GetParticlesInf(CreateParticelData data);
    }
}
