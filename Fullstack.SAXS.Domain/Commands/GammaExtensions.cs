using MathNet.Numerics.Distributions;

namespace Fullstack.SAXS.Domain.Commands
{
    public static class GammaExtensions
    {
        public static double GetGammaRandom(this Gamma gamma, double min, double max)
        {
            var randValue = gamma.Sample();

            while (randValue <= min || randValue >= max)
            {
                randValue = gamma.Sample();
            }

            return randValue;
        }
    }
}
