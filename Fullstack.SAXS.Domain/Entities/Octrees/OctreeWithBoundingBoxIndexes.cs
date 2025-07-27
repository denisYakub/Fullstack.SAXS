using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    public class OctreeWithBoundingBoxIndexes : IOctree
    {
        private readonly Dictionary<int, BoundingBox> _octaneDepth0 = new(8 + 1);

        private ICollection<(int index, Particle particle)> _particles = new List<(int index, Particle particle)>(1000);

        public OctreeWithBoundingBoxIndexes(double outerRadius)
        {
            var outerBox = new BoundingBox(outerRadius * 2, new(0, 0, 0));
            var index = -1;

            _octaneDepth0.Add(index++, outerBox);

            foreach (var box in outerBox.OctoSplit())
                _octaneDepth0.Add(index++, box);
        }

        public IEnumerable<Particle> GetAll()
        {
            return _particles
                .Select(p => p.particle);
        }

        public bool TryToAdd(Particle particle)
        {
            var id = -2;

            foreach (var index in _octaneDepth0)
                if (index.Value.Contains(particle))
                    id = index.Key;

            if (id == -2)
                return false;
            
            foreach (var prtcl in _particles)
            {
                if (prtcl.index != -1 && id != -1 && prtcl.index != id)
                {
                    continue;
                }
                else if (prtcl.particle.Intersect(particle))
                {
                    return false;
                }
            }

            _particles.Add((id, particle));

            return true;
        }
    }
}
