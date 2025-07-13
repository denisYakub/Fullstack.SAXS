namespace Fullstack.SAXS.Domain.Extensions
{
    public static class RandomExtensions
    {
        public static double GetEvenlyRandom(this Random random, double min, double max)
        {
            return min + (max - min) * random.NextDouble();
        }
    }
}
