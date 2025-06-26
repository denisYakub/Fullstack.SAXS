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
            throw new NotImplementedException();
        }

        public double LengthSquared()
        {
            throw new NotImplementedException();
        }

        public static Vector3D operator +(Vector3D a, Vector3D b)
        => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vector3D operator -(Vector3D a, Vector3D b)
            => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vector3D operator *(Vector3D v, double scalar) =>
        new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);

        public static Vector3D operator *(double scalar, Vector3D v) =>
            v * scalar;

        public static Vector3D Transform(Vector3D vector, Matrix4x4D matrix)
        {
            throw new NotImplementedException();
        }

        public static double Distance(Vector3D vector1, Vector3D vector2)
        {
            throw new NotImplementedException();
        }

        public static double DistanceSquared(Vector3D vector1, Vector3D vector2)
        {
            throw new NotImplementedException();
        }

        public static Vector3D Cross(Vector3D vector1, Vector3D vector2)
        {
            throw new NotImplementedException();
        }

        public static double Dot(Vector3D vector1, Vector3D vector2)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"<{x} {y} {z}>";
        }
    }
}
