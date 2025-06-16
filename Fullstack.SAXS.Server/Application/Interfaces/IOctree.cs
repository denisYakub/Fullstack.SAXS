using Fullstack.SAXS.Server.Domain.Entities.Particles;

namespace Fullstack.SAXS.Server.Application.Interfaces
{
    public interface IOctree<T> where T : Particle
    {
        public bool Add(Particle particle);
        public IEnumerable<T> GetAll();
    }
}
