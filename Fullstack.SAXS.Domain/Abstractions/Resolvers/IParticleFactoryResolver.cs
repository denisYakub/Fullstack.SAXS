using Fullstack.SAXS.Domain.Abstractions.Factories;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Abstractions.Resolvers
{
    public interface IParticleFactoryResolver
    {
        ParticleFactory Resolve(ParticleTypes type);
    }
}
