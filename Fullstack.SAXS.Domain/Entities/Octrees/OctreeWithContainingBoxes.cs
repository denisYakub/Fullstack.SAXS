using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    public class OctreeWithContainingBoxes : IOctree
    {
        private sealed class Node(BoundingBox box)
        {
            public Guid Id { get; } = Guid.NewGuid();
            public BoundingBox Box { get; } = box;
        }

        private Node[] _children;
        private List<(Particle particle, List<Guid> boxs)> _objects;

        public OctreeWithContainingBoxes(double outerRadius)
        {
            _children = SubDivide(new(outerRadius * 2, new(0, 0, 0)));
            _objects = new List<(Particle, List<Guid>)>(10000);
        }

        public bool TryToAdd(Particle particle)
        {
            var boxIds = new List<Guid>(9);
            foreach (var child in _children)
            {
                if (child.Box.Contains(particle) || child.Box.Clashes(particle))
                    boxIds.Add(child.Id);
            }

            var hasIntersection = false;
            object lockObj = new();

            Parallel.ForEach(_objects, (obj, state) =>
            {
                foreach (var boxId in boxIds)
                {
                    if (obj.boxs.Contains(boxId))
                    {
                        if (obj.particle.Intersect(particle))
                        {
                            lock (lockObj)
                                hasIntersection = true;

                            state.Stop();
                            return;
                        }
                    }
                }
            });

            if (!hasIntersection)
                _objects.Add((particle, boxIds));

            return !hasIntersection;
        }

        public IEnumerable<Particle> GetAll()
        {
            return _objects.Select(obj => obj.particle);
        }

        private static Node[] SubDivide(BoundingBox parentBox)
        {
            var nodes = new Node[8];

            var bounds = parentBox.OctoSplit();

            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = new(bounds[i]);

            return nodes;
        }
    }
}
