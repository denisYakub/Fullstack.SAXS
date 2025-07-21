namespace Fullstack.SAXS.Domain.ValueObjects
{
    public readonly struct Matrix4x4D : IEquatable<Matrix4x4D>
    {
        private readonly double m11, m12, m13, m14;
        private readonly double m21, m22, m23, m24;
        private readonly double m31, m32, m33, m34;
        private readonly double m41, m42, m43, m44;

        public double M11 => m11;
        public double M12 => m12;
        public double M13 => m13;
        public double M14 => m14;

        public double M21 => m21;
        public double M22 => m22;
        public double M23 => m23;
        public double M24 => m24;

        public double M31 => m31;
        public double M32 => m32;
        public double M33 => m33;
        public double M34 => m34;

        public double M41 => m41;
        public double M42 => m42;
        public double M43 => m43;
        public double M44 => m44;

        public Matrix4x4D(
            double m11, double m12, double m13, double m14,
            double m21, double m22, double m23, double m24,
            double m31, double m32, double m33, double m34,
            double m41, double m42, double m43, double m44)
        {
            this.m11 = m11; this.m12 = m12; this.m13 = m13; this.m14 = m14;
            this.m21 = m21; this.m22 = m22; this.m23 = m23; this.m24 = m24;
            this.m31 = m31; this.m32 = m32; this.m33 = m33; this.m34 = m34;
            this.m41 = m41; this.m42 = m42; this.m43 = m43; this.m44 = m44;
        }

        public static Matrix4x4D Identity => 
            new Matrix4x4D(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            );

        public static Matrix4x4D CreateRotationX(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            return new Matrix4x4D(
                1, 0, 0, 0,
                0, cos, -sin, 0,
                0, sin, cos, 0,
                0, 0, 0, 1
            );
        }

        public static Matrix4x4D CreateRotationY(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            return new Matrix4x4D(
                cos, 0, sin, 0,
                0, 1, 0, 0,
                -sin, 0, cos, 0,
                0, 0, 0, 1
            );
        }

        public static Matrix4x4D CreateRotationZ(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            return new Matrix4x4D(
                cos, -sin, 0, 0,
                sin, cos, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            );
        }

        public static Matrix4x4D operator *(Matrix4x4D a, Matrix4x4D b)
        {
            return new Matrix4x4D(
                a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31 + a.m14 * b.m41,
                a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32 + a.m14 * b.m42,
                a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33 + a.m14 * b.m43,
                a.m11 * b.m14 + a.m12 * b.m24 + a.m13 * b.m34 + a.m14 * b.m44,

                a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31 + a.m24 * b.m41,
                a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32 + a.m24 * b.m42,
                a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33 + a.m24 * b.m43,
                a.m21 * b.m14 + a.m22 * b.m24 + a.m23 * b.m34 + a.m24 * b.m44,

                a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31 + a.m34 * b.m41,
                a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32 + a.m34 * b.m42,
                a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33 + a.m34 * b.m43,
                a.m31 * b.m14 + a.m32 * b.m24 + a.m33 * b.m34 + a.m34 * b.m44,

                a.m41 * b.m11 + a.m42 * b.m21 + a.m43 * b.m31 + a.m44 * b.m41,
                a.m41 * b.m12 + a.m42 * b.m22 + a.m43 * b.m32 + a.m44 * b.m42,
                a.m41 * b.m13 + a.m42 * b.m23 + a.m43 * b.m33 + a.m44 * b.m43,
                a.m41 * b.m14 + a.m42 * b.m24 + a.m43 * b.m34 + a.m44 * b.m44
            );
        }

        public override bool Equals(object? obj)
        {
            return obj is Matrix4x4D other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + m11.GetHashCode();
                hash = hash * 31 + m12.GetHashCode();
                hash = hash * 31 + m13.GetHashCode();
                hash = hash * 31 + m14.GetHashCode();

                hash = hash * 31 + m21.GetHashCode();
                hash = hash * 31 + m22.GetHashCode();
                hash = hash * 31 + m23.GetHashCode();
                hash = hash * 31 + m24.GetHashCode();

                hash = hash * 31 + m31.GetHashCode();
                hash = hash * 31 + m32.GetHashCode();
                hash = hash * 31 + m33.GetHashCode();
                hash = hash * 31 + m34.GetHashCode();

                hash = hash * 31 + m41.GetHashCode();
                hash = hash * 31 + m42.GetHashCode();
                hash = hash * 31 + m43.GetHashCode();
                hash = hash * 31 + m44.GetHashCode();

                return hash;
            }
        }

        public static bool operator ==(Matrix4x4D left, Matrix4x4D right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Matrix4x4D left, Matrix4x4D right)
        {
            return !(left == right);
        }

        public bool Equals(Matrix4x4D other)
        {
            return
                m11 == other.m11 && m12 == other.m12 && m13 == other.m13 && m14 == other.m14 &&
                m21 == other.m21 && m22 == other.m22 && m23 == other.m23 && m24 == other.m24 &&
                m31 == other.m31 && m32 == other.m32 && m33 == other.m33 && m34 == other.m34 &&
                m41 == other.m41 && m42 == other.m42 && m43 == other.m43 && m44 == other.m44;
        }

        public static Matrix4x4D Multiply(Matrix4x4D left, Matrix4x4D right)
        {
            throw new NotImplementedException();
        }
    }

}
