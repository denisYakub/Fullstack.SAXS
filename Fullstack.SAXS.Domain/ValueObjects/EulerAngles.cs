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
        public readonly double PraecessioAngle;
        public readonly double NutatioAngle;
        public readonly double ProperRotationAngle;

        public EulerAngles(double alpha, double betta, double gamma)
        {
            PraecessioAngle = alpha;
            NutatioAngle = betta;
            ProperRotationAngle = gamma;
        }

        public Matrix4x4D CreateRotationMatrix()
        {
            var alpha = Math.PI * PraecessioAngle / 180.0;
            var beta = Math.PI * NutatioAngle / 180.0;
            var gamma = Math.PI * ProperRotationAngle / 180.0;

            var rotationX = Matrix4x4D.CreateRotationX(alpha);
            var rotationY = Matrix4x4D.CreateRotationY(beta);
            var rotationZ = Matrix4x4D.CreateRotationZ(gamma);

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
                    double.Parse(
                        data[0].Replace(',', '.'), 
                        CultureInfo.InvariantCulture
                    ),
                    double.Parse(
                        data[1].Replace(',', '.'), 
                        CultureInfo.InvariantCulture
                    ),
                    double.Parse(
                        data[2].Replace(',', '.'), 
                        CultureInfo.InvariantCulture
                    )
                );
        }
    }
}
