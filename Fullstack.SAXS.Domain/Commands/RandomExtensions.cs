namespace Fullstack.SAXS.Server.Domain.Commands
{
    public static class RandomExtensions
    {
        public static float GetEvenlyRandom(this Random random, float min, float max)
        {
            return min + (max - min) * (float)random.NextDouble();
        }
    }
}
