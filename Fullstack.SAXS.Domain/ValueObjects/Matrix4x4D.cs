using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.SAXS.Domain.ValueObjects
{
    public struct Matrix4x4D
    {
        public double M11, M12, M13, M14;
        public double M21, M22, M23, M24;
        public double M31, M32, M33, M34;
        public double M41, M42, M43, M44;

        public Matrix4x4D(
            double m11, double m12, double m13, double m14,
            double m21, double m22, double m23, double m24,
            double m31, double m32, double m33, double m34,
            double m41, double m42, double m43, double m44)
        {
            M11 = m11; M12 = m12; M13 = m13; M14 = m14;
            M21 = m21; M22 = m22; M23 = m23; M24 = m24;
            M31 = m31; M32 = m32; M33 = m33; M34 = m34;
            M41 = m41; M42 = m42; M43 = m43; M44 = m44;
        }

        public static Matrix4x4D Identity => new Matrix4x4D(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);

        public static Matrix4x4D CreateRotationX(double angle)
        {
            throw new NotImplementedException();
        }

        public static Matrix4x4D CreateRotationY(double angle)
        {
            throw new NotImplementedException();
        }

        public static Matrix4x4D CreateRotationZ(double angle)
        {
            throw new NotImplementedException();
        }

        public static Matrix4x4D operator *(Matrix4x4D m1, Matrix4x4D m2)
        {
            throw new NotImplementedException();
        }
    }
}
