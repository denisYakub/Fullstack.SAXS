using System.Numerics;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Domain.Entities.Particles
{
    public abstract class Particle
    {
        public readonly double Size;
        public readonly Vector3D Center;
        public readonly EulerAngles RotationAngles;

        public abstract double OuterSphereRadius { get; }
        public abstract double InnerSphereRadius { get; }
        public abstract IReadOnlyCollection<Vector3D> Vertices { get; }
        public abstract ParticleTypes ParticleType { get; }
        protected abstract int[][] Faces { get; }
        protected abstract double Volume { get; }

        protected Particle(double size, Vector3D center, EulerAngles rotationAngles)
        {
            Size = size;
            Center = center;
            RotationAngles = rotationAngles;
        }

        public bool Intersect(Particle other)
        {
            var outerRSum = OuterSphereRadius + other.OuterSphereRadius;

            if (Vector3D.DistanceSquared(Center, other.Center) > outerRSum * outerRSum)
                return false;

            var innerRSum = InnerSphereRadius + other.InnerSphereRadius;

            if (Vector3D.DistanceSquared(Center, other.Center) <= innerRSum * innerRSum)
                return true;

            foreach (var vertex in other.Vertices)
                if (Contains(vertex))
                    return true;

            return false;
        }

        public bool Contains(Vector3D point)
        {
            if (Vector3D.DistanceSquared(point, Center) > OuterSphereRadius * OuterSphereRadius)
                return false;

            if (Vector3D.DistanceSquared(point, Center) <= InnerSphereRadius * InnerSphereRadius)
                return true;

            var vertices = Vertices;

            foreach (var face in Faces)
            {
                var a = vertices.ElementAt(face[0]);
                var b = vertices.ElementAt(face[1]);
                var c = vertices.ElementAt(face[2]);

                var normal = Vector3D.Cross(b - a, c - a);

                var dotProduct = Vector3D.Dot(normal, point - a);

                if (dotProduct > 0.0)
                {
                    return false;
                }
            }

            return true;
        }

        /*public string Draw()
        {
            var strB = new StringBuilder();
            var str = new string[12];

            for (int i = 0; i < str.Length; i++)
            {
                string input = Vertices.ElementAt(i).ToString();

                str[i] = 
                    "[" + 
                    string
                        .Join(
                            ", ",
                            input
                                .Trim('<', '>')
                                .Split([' ', '\u00A0'], StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => s.Replace(',', '.'))
                        )
                     + "]";
            }
            strB.AppendJoin(", ", str);

            return strB.ToString();
        }*/
    }
}
