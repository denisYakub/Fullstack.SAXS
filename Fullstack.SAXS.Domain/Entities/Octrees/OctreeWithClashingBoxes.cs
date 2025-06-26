using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    public class OctreeWithClashingBoxes(float sphereRadius) : IOctree
    {
        class Node
        {
            private readonly static int MaxSize = 1000;
            private readonly static int MaxDepth = 6;
            private BoundingBox _bounds;
            private List<Particle> _particles = new(MaxSize);
            private Node[]? _children;
            public bool IsLeaf
            {
                get { return _children == null; }
            }

            public Node(BoundingBox bounds)
            {
                _bounds = bounds;
            }

            public bool Insert(Particle particle, int depth = 0)
            {
                if (IsLeaf)
                {
                    if (_particles.Count < MaxSize || depth >= MaxDepth)
                    {
                        if (!_particles.AsParallel().Any(particle.Intersect))
                        {
                            _particles.Add(particle);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    SubDivide();
                }

                var targetChildren = _children!.Where(child => child._bounds.Clashes(particle)).ToList();

                foreach (var child in targetChildren)
                    if (child._particles.AsParallel().Any(particle.Intersect))
                        return false;

                foreach (var child in targetChildren)
                    child.Insert(particle, depth + 1);

                return true;
            }

            public IEnumerable<Particle> PullOut()
            {
                var nodes = new Stack<Node>();
                nodes.Push(this);

                while (nodes.Any())
                {
                    var node = nodes.Pop();
                    var nodeParticles = node._particles;

                    if (!node.IsLeaf)
                    {
                        var nodeChildren = node._children;

                        foreach (var child in nodeChildren)
                            nodes.Push(child);
                    }

                    foreach (var particle in nodeParticles)
                        yield return particle;
                }
            }

            private void SubDivide()
            {
                _children = new Node[8];

                var bounds = _bounds.OctoSplit();

                for (int i = 0; i < _children.Length; i++)
                    _children[i] = new(bounds[i]);
            }
        }
        private readonly Node _root = new (new (sphereRadius * 2, new (0, 0, 0)));
        private List<Particle> _particles = new (10000);

        public bool Add(Particle particle)
        {
            if (_root.Insert(particle))
            {
                _particles.Add(particle);
                return true;
            }

            return false;
        }

        public IEnumerable<Particle> GetAll()
        {
            return _particles;
        }
    }
}
