using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IOctree
    {
        bool TryToAdd(Particle particle);
        IEnumerable<Particle> GetAll();
    }
}
