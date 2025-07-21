using System.Globalization;

namespace Fullstack.SAXS.Domain.ValueObjects
{
    /// <summary>
    /// Rotation angles:
    /// PraecessioAngle - Alpha
    /// NutatioAngle - Betta
    /// ProperRotationAngle - Gamma
    /// </summary>
    public readonly struct EulerAngles : IEquatable<EulerAngles>
    {
        private readonly double praecessioAngle;
        private readonly double nutatioAngle;
        private readonly double properRotationAngle;

        public double PraecessioAngle => praecessioAngle;
        public double NutatioAngle => nutatioAngle;
        public double ProperRotationAngle => properRotationAngle;

        public EulerAngles(double alpha, double betta, double gamma)
        {
            praecessioAngle = alpha;
            nutatioAngle = betta;
            properRotationAngle = gamma;
        }

        public Matrix4x4D CreateRotationMatrix()
        {
            var alpha = Math.PI * praecessioAngle / 180.0;
            var beta = Math.PI * nutatioAngle / 180.0;
            var gamma = Math.PI * properRotationAngle / 180.0;

            var rotationX = Matrix4x4D.CreateRotationX(alpha);
            var rotationY = Matrix4x4D.CreateRotationY(beta);
            var rotationZ = Matrix4x4D.CreateRotationZ(gamma);

            return rotationX * rotationY * rotationZ;
        }

        public override string ToString()
        {
            return $"<{praecessioAngle} {nutatioAngle} {properRotationAngle}>";
        }

        public static EulerAngles Parse(string s)
        {
            try
            {
                if (s is null)
                    throw new ArgumentNullException(nameof(s), "Input string cannot be null.");

                if (s[0] != '<' && s[s.Length - 1] != '>')
                    throw new FormatException("Input must start with '<' and end with '>'.");

                var values = s.Substring(1, s.Length - 2);
                var data = values.Split(' ');

                if (data.Length != 3)
                    throw new FormatException("Input must contain exactly three space-separated values.");

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
            catch (FormatException ex)
            {
                throw new FormatException("One or more values are not valid floating-point numbers.", ex);
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("One or more values are too large or too small.", ex);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is EulerAngles other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + praecessioAngle.GetHashCode();
                hash = hash * 31 + nutatioAngle.GetHashCode();
                hash = hash * 31 + properRotationAngle.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(EulerAngles left, EulerAngles right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EulerAngles left, EulerAngles right)
        {
            return !(left == right);
        }

        public bool Equals(EulerAngles other)
        {
            return praecessioAngle == other.praecessioAngle &&
                nutatioAngle == other.nutatioAngle &&
                properRotationAngle == other.properRotationAngle;
        }
    }
}
