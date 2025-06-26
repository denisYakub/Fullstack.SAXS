using Fullstack.SAXS.Domain.Entities.Particles;
using System.Numerics;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    public class BoundingBox
    {
        public float Edge {  get; init; }
        public Vector3 Center { get; init; }

        public BoundingBox(float edge, Vector3 center)
        {
            Edge = edge;
            Center = center;
        }

        public BoundingBox[] OctoSplit()
        {
            var halfEdge = Edge / 2;
            var dCoord = Edge / 4;

            return [
                new (halfEdge, new (Center.X - dCoord, Center.Y - dCoord, Center.Z - dCoord)),
                new (halfEdge, new (Center.X - dCoord, Center.Y - dCoord, Center.Z + dCoord)),
                new (halfEdge, new (Center.X - dCoord, Center.Y + dCoord, Center.Z - dCoord)),
                new (halfEdge, new (Center.X - dCoord, Center.Y + dCoord, Center.Z + dCoord)),
                new (halfEdge, new (Center.X + dCoord, Center.Y - dCoord, Center.Z - dCoord)),
                new (halfEdge, new (Center.X + dCoord, Center.Y - dCoord, Center.Z + dCoord)),
                new (halfEdge, new (Center.X + dCoord, Center.Y + dCoord, Center.Z - dCoord)),
                new (halfEdge, new (Center.X + dCoord, Center.Y + dCoord, Center.Z + dCoord)),
            ];
        }

        public bool Contains(Vector3 point)
        {
            var half = new Vector3(Edge / 2);
            var min = Center - half;
            var max = Center + half;

            return point.X >= min.X && point.X <= max.X &&
                   point.Y >= min.Y && point.Y <= max.Y &&
                   point.Z >= min.Z && point.Z <= max.Z;
        }

        public bool Contains(Particle particle)
        {
            var half = Edge / 2;

            return
                particle.Center.X - particle.OuterSphereRadius >= Center.X - half &&
                particle.Center.X + particle.OuterSphereRadius <= Center.X + half &&
                particle.Center.Y - particle.OuterSphereRadius >= Center.Y - half &&
                particle.Center.Y + particle.OuterSphereRadius <= Center.Y + half &&
                particle.Center.Z - particle.OuterSphereRadius >= Center.Z - half &&
                particle.Center.Z + particle.OuterSphereRadius <= Center.Z + half;
        }

        public bool Clashes(Particle particle)
        {
            Vector3 cubeMin = Center - new Vector3(Edge / 2);
            Vector3 cubeMax = Center + new Vector3(Edge / 2);

            float x = MathF.Max(cubeMin.X, MathF.Min(particle.Center.X, cubeMax.X));
            float y = MathF.Max(cubeMin.Y, MathF.Min(particle.Center.Y, cubeMax.Y));
            float z = MathF.Max(cubeMin.Z, MathF.Min(particle.Center.Z, cubeMax.Z));

            float distanceSquared = (particle.Center - new Vector3(x, y, z)).LengthSquared();

            if (distanceSquared > particle.OuterSphereRadius * particle.OuterSphereRadius)
                return false;

            if (distanceSquared <= particle.InnerSphereRadius * particle.InnerSphereRadius)
                return true;

            foreach (var vertex in particle.Vertices)
                if (Contains(vertex))
                    return true;

            return false;
        }
    }
}
