using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.SAXS.Domain.ValueObjects
{
    public struct Vector3D
    {
        private readonly double x, y, z;

        public double X => x;
        public double Y => y;
        public double Z => z;

        public Vector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3D(double coord)
        {
            this.x = coord;
            this.y = coord;
            this.z = coord;
        }

        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        public double LengthSquared()
        {
            return x * x + y * y + z * z;
        }

        public static Vector3D operator +(Vector3D a, Vector3D b)
            => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vector3D operator -(Vector3D a, Vector3D b)
            => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vector3D operator *(Vector3D v, double scalar)
            => new(v.X * scalar, v.Y * scalar, v.Z * scalar);

        public static Vector3D operator *(double scalar, Vector3D v)
            => v * scalar;

        public static Vector3D Transform(Vector3D vector, Matrix4x4D matrix)
        {
            double x = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + matrix.M41;
            double y = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + matrix.M42;
            double z = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + matrix.M43;
            double w = vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + matrix.M44;

            if (Math.Abs(w) > double.Epsilon && w != 1.0)
            {
                x /= w;
                y /= w;
                z /= w;
            }

            return new Vector3D(x, y, z);
        }

        public static double Distance(Vector3D vector1, Vector3D vector2)
        {
            return Math.Sqrt(DistanceSquared(vector1, vector2));
        }

        public static double DistanceSquared(Vector3D vector1, Vector3D vector2)
        {
            double dx = vector1.X - vector2.X;
            double dy = vector1.Y - vector2.Y;
            double dz = vector1.Z - vector2.Z;
            return dx * dx + dy * dy + dz * dz;
        }

        public static Vector3D Cross(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(
                vector1.Y * vector2.Z - vector1.Z * vector2.Y,
                vector1.Z * vector2.X - vector1.X * vector2.Z,
                vector1.X * vector2.Y - vector1.Y * vector2.X
            );
        }

        public static double Dot(Vector3D vector1, Vector3D vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        public override string ToString()
        {
            return $"<{x} {y} {z}>";
        }
    }

}
