using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IOctree
    {
        bool Add(Particle particle);
        IEnumerable<Particle> GetAll();
    }
}
