using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.ValueObjects;
using System.Numerics;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    public class BoundingBox
    {
        public double Edge {  get; init; }
        public Vector3D Center { get; init; }

        public BoundingBox(double edge, Vector3D center)
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

        public bool Contains(Vector3D point)
        {
            var half = new Vector3D(Edge / 2);
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
            var cubeMin = Center - new Vector3D(Edge / 2);
            var cubeMax = Center + new Vector3D(Edge / 2);

            var x = Math.Max(cubeMin.X, Math.Min(particle.Center.X, cubeMax.X));
            var y = Math.Max(cubeMin.Y, Math.Min(particle.Center.Y, cubeMax.Y));
            var z = Math.Max(cubeMin.Z, Math.Min(particle.Center.Z, cubeMax.Z));

            var distanceSquared = (particle.Center - new Vector3D(x, y, z)).LengthSquared();

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
