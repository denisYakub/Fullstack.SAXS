using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Domain.Entities.Particles
{
    public abstract class Particle(double size, Vector3D center, EulerAngles rotationAngles)
    {
        public double Size => size;
        public Vector3D Center => center;
        public EulerAngles RotationAngles => rotationAngles;

        public abstract double OuterSphereRadius { get; }
        public abstract double InnerSphereRadius { get; }
        public abstract IReadOnlyCollection<Vector3D> Vertices { get; }
        public abstract ParticleTypes ParticleType { get; }
        protected abstract IReadOnlyCollection<int[]> Faces { get; }

        public bool Intersect(Particle other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "Shouldn't be null.");

            var outerRSum = OuterSphereRadius + other.OuterSphereRadius;

            if (Vector3D.DistanceSquared(center, other.Center) > outerRSum * outerRSum)
                return false;

            var innerRSum = InnerSphereRadius + other.InnerSphereRadius;

            if (Vector3D.DistanceSquared(center, other.Center) <= innerRSum * innerRSum)
                return true;

            foreach (var vertex in other.Vertices)
                if (Contains(vertex))
                    return true;

            foreach (var vertex in Vertices)
                if (other.Contains(vertex))
                    return true;

            return false;
        }

        public bool Contains(Vector3D point)
        {
            if (Vector3D.DistanceSquared(point, center) > OuterSphereRadius * OuterSphereRadius)
                return false;

            if (Vector3D.DistanceSquared(point, center) <= InnerSphereRadius * InnerSphereRadius)
                return true;

            var vertices = Vertices;

            foreach (var face in Faces)
            {
                var a = vertices.ElementAt(face[0]);
                var b = vertices.ElementAt(face[1]);
                var c = vertices.ElementAt(face[2]);

                var normal = Vector3D.Cross(b - a, c - a);

                var dotProduct = Vector3D.Dot(normal, point - a);

                if (dotProduct > 1e-6)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
