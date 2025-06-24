using System.Globalization;
using System.Numerics;

namespace Fullstack.SAXS.Domain.ValueObjects
{
    /// <summary>
    /// Rotation angles:
    /// PraecessioAngle - Alpha
    /// NutatioAngle - Betta
    /// ProperRotationAngle - Gamma
    /// </summary>
    public readonly struct EulerAngles
    {
        public readonly float PraecessioAngle;
        public readonly float NutatioAngle;
        public readonly float ProperRotationAngle;

        public EulerAngles(float alpha, float betta, float gamma)
        {
            PraecessioAngle = alpha;
            NutatioAngle = betta;
            ProperRotationAngle = gamma;
        }

        public Matrix4x4 CreateRotationMatrix()
        {
            float alpha = MathF.PI * PraecessioAngle / 180.0f;
            float beta = MathF.PI * NutatioAngle / 180.0f;
            float gamma = MathF.PI * ProperRotationAngle / 180.0f;

            var rotationX = Matrix4x4.CreateRotationX(alpha);
            var rotationY = Matrix4x4.CreateRotationY(beta);
            var rotationZ = Matrix4x4.CreateRotationZ(gamma);

            return rotationX * rotationY * rotationZ;
        }

        public override string ToString()
        {
            return $"<{PraecessioAngle} {NutatioAngle} {ProperRotationAngle}>";
        }

        public static EulerAngles Parse(string s)
        {
            if (s[0] != '<' && s[s.Length - 1] != '>')
                throw new ArgumentException("Bad format!");

            var values = s.Substring(1, s.Length - 2);
            var data = values.Split(' ');

            if (data.Length != 3)
                throw new ArgumentException("Bad format!");

            return 
                new EulerAngles(
                    float.Parse(
                        data[0].Replace(',', '.'), 
                        CultureInfo.InvariantCulture
                    ),
                    float.Parse(
                        data[1].Replace(',', '.'), 
                        CultureInfo.InvariantCulture
                    ),
                    float.Parse(
                        data[2].Replace(',', '.'), 
                        CultureInfo.InvariantCulture
                    )
                );
        }
    }
}
