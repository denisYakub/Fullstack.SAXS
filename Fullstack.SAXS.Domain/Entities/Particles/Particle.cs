using System.Numerics;
using System.Text;
using Fullstack.SAXS.Domain.Commands;
using Fullstack.SAXS.Server.Domain.Enums;
using Fullstack.SAXS.Server.Domain.ValueObjects;

namespace Fullstack.SAXS.Server.Domain.Entities.Particles
{
    public abstract class Particle
    {
        public readonly float Size;
        public readonly Vector3 Center;
        public readonly EulerAngles RotationAngles;

        public abstract float OuterSphereRadius { get; }
        public abstract float InnerSphereRadius { get; }
        public abstract IReadOnlyCollection<Vector3> Vertices { get; }
        public abstract ParticleTypes ParticleType { get; }
        protected abstract int[][] Faces { get; }
        protected abstract float Volume { get; }

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

            foreach (var vertex in other.Vertices)
                if (Contains(vertex))
                    return true;

            return false;
        }

        public bool Contains(Vector3 point)
        {
            var centerF = Center;
            var vertices = Vertices;

            if (FiguresCollision.Pointinside(in centerF, InnerSphereRadius, in point))
                return true;

            if (!FiguresCollision.Pointinside(in centerF, OuterSphereRadius, in point))
                return false;

            foreach (var face in Faces)
            {
                Vector3 a = vertices.ElementAt(face[0]);
                Vector3 b = vertices.ElementAt(face[1]);
                Vector3 c = vertices.ElementAt(face[2]);

                Vector3 normal = Vector3.Cross(b - a, c - a);

                float dotProduct = Vector3.Dot(normal, point - a);

                if (dotProduct > 0.0f)
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
