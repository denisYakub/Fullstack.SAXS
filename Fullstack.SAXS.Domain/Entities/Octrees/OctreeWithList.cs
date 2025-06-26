using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    public class OctreeWithList(double outerRadius) : IOctree
    {
        private List<Particle> _particles = new (10000);
        public bool Add(Particle particle)
        {
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
