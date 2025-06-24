using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Entities.Regions;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    public class Octree
    {
        class Node
        {
            public readonly int Depth;

            private Region _region;
            private Node?[] _children;
            private ICollection<Particle> _particles;

            public IEnumerable<Node?> Children => _children;

            public Node(Region region, int depth)
            {
                _region = region;
                Depth = depth;
                _children = new Node?[8];
                _particles = new List<Particle>(1000);
            }

            public void SubDivide(int depth)
            {
                var subRegions = _region.OctoSplit();

                for (int i = 0; i < subRegions.Length; i++)
                    _children[i] = new Node(subRegions[i], depth++);
            }

            public bool Contains(Particle particle) =>
                _region.Contains(particle);

            public bool Intersect(Particle particle) =>
                _particles.AsParallel().Any(particle.Intersect);

            public void Add(Particle particle) =>
                _particles.Add(particle);

            public IEnumerable<Particle> GetList() =>
                _particles;
        }

        public readonly int MaxDepth;
        private readonly Node _root;

        public Octree(int maxDepth, Region startRegion)
        {
            MaxDepth = maxDepth;
            _root = new Node(startRegion, 0);
        }

        public bool Add(Particle particle)
        {
            var stack = new Stack<Node>();
            stack.Push(_root);

            while (stack.Any())
            {
                var current = stack.Pop();

                if (!current.Contains(particle))
                    continue;

                if (current.Intersect(particle))
                    return false;

                if (current.Depth == MaxDepth)
                {
                    current.Add(particle);

                    return true;
                }

                if (current.Children.Any(child => child is null))
                    current.SubDivide(current.Depth);

                bool pushedToChild = false;
                foreach (var child in current.Children)
                {
                    if (child.Contains(particle))
                    {
                        stack.Push(child);
                        pushedToChild = true;
                    }
                }

                if (!pushedToChild)
                {
                    current.Add(particle);

                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Particle> GetAll()
        {
            var stack = new Stack<Node>();
            stack.Push(_root);

            while (stack.Any())
            {
                var current = stack.Pop();

                foreach (var particle in current.GetList())
                    yield return particle;

                foreach (var child in current.Children)
                    if (child != null)
                        stack.Push(child);
            }
        }
    }
}
