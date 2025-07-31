using Fullstack.SAXS.Domain.Common;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    public class OctreeWithEnumId : IOctree
    {
        private readonly Bound _box;
        private ICollection<(OctantId octant, Particle value)> _particles = new List<(OctantId, Particle)> (10000);

        public OctreeWithEnumId(double outerRaius)
        {
            _box = new(outerRaius, new(0, 0, 0));
        }

        public IEnumerable<Particle> GetAll() =>
            _particles
            .Select(p => p.value);

        public bool TryToAdd(Particle particle)
        {
            if (particle == null)
                throw new ArgumentNullException(nameof(particle), "Shouldn't be null.");

            if (!_box.Contains(particle))
                return false;

            var octant = _box.GetIdOfContainingSubBound(particle);

            bool hasIntersection = false;
            object lockObj = new();

            Parallel.ForEach(_particles, (prtcl, state) =>
            {
                if (octant != OctantId.None && prtcl.octant != OctantId.None && octant != prtcl.octant)
                {
                    return;
                }

                if (prtcl.value.Intersect(particle))
                {
                    lock (lockObj)
                        hasIntersection = true;

                    state.Stop();
                }
            });

            if (!hasIntersection)
                _particles.Add((octant, particle));

            return hasIntersection;
        }
    }
}
