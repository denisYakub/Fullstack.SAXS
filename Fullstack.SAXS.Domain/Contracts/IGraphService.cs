namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IGraphService
    {
        Task<string> GetHtmlPageAsync(float[] x, float[] y);
    }
}
