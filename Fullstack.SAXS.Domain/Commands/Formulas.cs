using System.Numerics;

namespace Fullstack.SAXS.Domain.Commands
{
    public static class Formulas
    {
        public static Matrix4x4 CreateRotationMatrix(float angleA, float angleB, float angleG)
        {
            float alpha = MathF.PI * angleA / 180.0f;
            float beta = MathF.PI * angleB / 180.0f;
            float gamma = MathF.PI * angleG / 180.0f;

            var rotationX = Matrix4x4.CreateRotationX(alpha);
            var rotationY = Matrix4x4.CreateRotationY(beta);
            var rotationZ = Matrix4x4.CreateRotationZ(gamma);

            return rotationX * rotationY * rotationZ;
        }
        public static Matrix4x4 CreateRotationMatrixReverse(float angleA, float angleB, float angleG)
        {
            float alpha = -MathF.PI * angleA / 180.0f;
            float beta = -MathF.PI * angleB / 180.0f;
            float gamma = -MathF.PI * angleG / 180.0f;

            var rotationX = Matrix4x4.CreateRotationX(alpha);
            var rotationY = Matrix4x4.CreateRotationY(beta);
            var rotationZ = Matrix4x4.CreateRotationZ(gamma);

            return rotationX * rotationY * rotationZ;
        }
        public static Vector3 GetNormal(Vector3 v1, Vector3 v2, Vector3 v3)
            => Vector3.Normalize(Vector3.Cross(v2 - v1, v3 - v1));
        public static bool IsSeparatingAxis(Vector3 axis, ICollection<Vector3> vertices1, ICollection<Vector3> vertices2)
        {
            (float min1, double max1) = GetProjection(vertices1, axis);
            (float min2, float max2) = GetProjection(vertices2, axis);

            return max1 < min2 || max2 < min1;
        }
        public static (float min, float max) GetProjection(ICollection<Vector3> vertices, Vector3 axes)
        {
            ArgumentNullException.ThrowIfNull(vertices);

            float min = float.MaxValue;
            float max = float.MinValue;

            for (int i = 0; i < vertices.Count; i++)
            {
                float p = Vector3.Dot(vertices.ElementAt(i), axes);
                min = Math.Min(min, p);
                max = Math.Max(max, p);
            }

            return (min, max);
        }
    }
}
