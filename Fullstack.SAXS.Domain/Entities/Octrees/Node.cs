using System.Numerics;
using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    internal class Node
    {
        private readonly static int MaxSize = 1000;
        private readonly static int MaxDepth = 6;
        private BoundingBox _bounds;
        private List<Particle> _particles = new (MaxSize);
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

            bool inserted = false;

            var targetChildren = _children.Where(child => child.Clashes(particle)).ToList();

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

                foreach(var particle in nodeParticles)
                    yield return particle;
            }
        }

        public bool Clashes(Particle particle)
        {
            Vector3 cubeMin = _bounds.Center - new Vector3(_bounds.Edge / 2);
            Vector3 cubeMax = _bounds.Center + new Vector3(_bounds.Edge / 2);

            float x = MathF.Max(cubeMin.X, MathF.Min(particle.Center.X, cubeMax.X));
            float y = MathF.Max(cubeMin.Y, MathF.Min(particle.Center.Y, cubeMax.Y));
            float z = MathF.Max(cubeMin.Z, MathF.Min(particle.Center.Z, cubeMax.Z));

            float distanceSquared = (particle.Center - new Vector3(x, y, z)).LengthSquared();

            if (distanceSquared > particle.OuterSphereRadius * particle.OuterSphereRadius)
                return false;

            if (distanceSquared <= particle.InnerSphereRadius * particle.InnerSphereRadius)
                return true;

            foreach (var vertex in particle.Vertices)
                if (_bounds.Contains(vertex))
                    return true;

            return false;
        }

        private void SubDivide()
        {
            _children = new Node[8];

            var bounds = _bounds.OctoSplit();

            for (int i = 0; i < _children.Length; i++)
                _children[i] = new (bounds[i]);
        }
    }
}  