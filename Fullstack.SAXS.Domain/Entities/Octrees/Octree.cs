using System.Numerics;
using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    public class Octree(float sphereRadius)
    {
        private readonly Node _root = new (new (sphereRadius * 2, new (0, 0, 0)));

        public bool Add(Particle particle)
        {
            return _root.Insert(particle);
        }

        public IReadOnlyCollection<Particle> GetAll()
        {
            return [.. _root.PullOut()];
        }
    }
}
