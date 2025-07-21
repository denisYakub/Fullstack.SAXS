namespace Fullstack.SAXS.Domain.Extensions
{
    public static class RandomExtensions
    {
        public static double GetEvenlyRandom(this Random random, double min, double max)
        {
            if (random == null) 
                throw new ArgumentNullException(nameof(random), "Shouldn't be null.");

            return min + (max - min) * random.NextDouble();
        }
    }
}
