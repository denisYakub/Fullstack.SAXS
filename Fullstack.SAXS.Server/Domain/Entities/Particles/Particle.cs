using System.Numerics;
using Fullstack.SAXS.Server.Domain.Enums;
using Fullstack.SAXS.Server.Domain.ValueObjects;

namespace Fullstack.SAXS.Server.Domain.Entities.Particles
{
    public abstract class Particle
    {
        public readonly float Size;
        public readonly Vector3 Center;
        public readonly EulerAngles RotationAngles;

        public abstract float Volume { get; }
        public abstract float OuterSphereRadius { get; }
        public abstract float InnerSphereRadius { get; }
        public abstract ReadOnlySpan<int[]> Faces { get; }
        public abstract ParticleTypes ParticleType { get; }
        public abstract ReadOnlySpan<Vector3> Vertices { get; }

        protected Particle(float size, Vector3 center, EulerAngles rotationAngles)
        {
            Size = size;
            Center = center;
            RotationAngles = rotationAngles;
        }

        public bool Intersect(Particle other)
        {
            var innerRSum = InnerSphereRadius + other.InnerSphereRadius;

            if (Vector3.DistanceSquared(Center, other.Center) <= innerRSum * innerRSum)
                return true;

            var outerRSum = OuterSphereRadius + other.OuterSphereRadius;

            if (Vector3.DistanceSquared(Center, other.Center) > outerRSum * outerRSum)
                return false;

            foreach (var vertex in Vertices)
                if (other.Contains(vertex))
                    return true;

            return false;
        }

        public bool Contains(Vector3 point)
        {
            var vertices = Vertices;

            foreach (var face in Faces)
            {
                Vector3 a = vertices[face[0]];
                Vector3 b = vertices[face[1]];
                Vector3 c = vertices[face[2]];

                var ab = b - a;
                var ac = c - a;
                var normal = Vector3.Cross(ab, ac);

                var ap = point - a;
                var ao = Center - a;

                var dot1 = Vector3.Dot(normal, ap);
                var dot2 = Vector3.Dot(normal, ao);

                if (Vector3.Dot(normal, ap) * Vector3.Dot(normal, ao) >= 0.0f)
                    return true;
            }

            return false;
        }
    }
}
