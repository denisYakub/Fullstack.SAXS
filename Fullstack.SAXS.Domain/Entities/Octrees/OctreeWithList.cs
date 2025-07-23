using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    internal class OctreeWithList : IOctree
    {
        private List<Particle> _particles = new (10000);
        public bool TryToAdd(Particle particle)
        {
            if (particle == null)
                throw new ArgumentNullException(nameof(particle), "Shouldn't be null.");

            if (_particles.AsParallel().Any(particle.Intersect))
                return false;

            _particles.Add(particle);

            return true;
        }

        public IEnumerable<Particle> GetAll()
        {
            return _particles;
        }
    }
}
