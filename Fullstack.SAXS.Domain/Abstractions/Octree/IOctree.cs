using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Abstractions.Octree
{
    public interface IOctree
    {
        bool TryToAdd(Particle particle);
        IEnumerable<Particle> GetAll();
    }
}
