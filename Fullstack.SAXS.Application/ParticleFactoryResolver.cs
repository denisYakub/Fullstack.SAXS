using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Application
{
    public class ParticleFactoryResolver(IEnumerable<ParticleFactory> factories) : IParticleFactoryResolver
    {
        private readonly Dictionary<ParticleTypes, ParticleFactory> _factories =
            factories?.ToDictionary(f => f.Type)
            ?? throw new ArgumentNullException(nameof(factories), "Particle factory resolver does not contains factories.");

        public ParticleFactory Resolve(ParticleTypes type)
            => _factories.TryGetValue(type, out var factory)
                ? factory
                : throw new NotSupportedException($"Particle type '{type}' is not supported.");
    }
}
