using MathNet.Numerics.Distributions;

namespace Fullstack.SAXS.Server.Domain.Commands
{
    public static class GammaExtensions
    {
        public static float GetGammaRandom(this Gamma gamma, float min, float max)
        {
            float randValue = (float)gamma.Sample();

            while (randValue <= min || randValue >= max)
            {
                randValue = (float)gamma.Sample();
            }

            return randValue;
        }
    }
}
