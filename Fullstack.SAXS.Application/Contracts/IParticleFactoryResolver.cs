using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Application.Contracts
{
    public interface IParticleFactoryResolver
    {
        ParticleFactory Resolve(ParticleTypes type);
    }
}
