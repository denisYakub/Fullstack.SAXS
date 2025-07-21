using System;
using MathNet.Numerics.Distributions;

namespace Fullstack.SAXS.Domain.Extensions
{
    public static class GammaExtensions
    {
        public static double GetGammaRandom(this Gamma gamma, double min, double max)
        {
            if (gamma == null)
                throw new ArgumentNullException(nameof(gamma), "Shouldn't be null.");

            var randValue = gamma.Sample();

            while (randValue <= min || randValue >= max)
            {
                randValue = gamma.Sample();
            }

            return randValue;
        }
    }
}
