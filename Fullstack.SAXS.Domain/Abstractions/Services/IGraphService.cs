namespace Fullstack.SAXS.Domain.Abstractions.Services
{
    public interface IGraphService
    {
        Task<string> GetHtmlPageAsync(double[] x, double[] y, string xLable, string yLable, string title);
    }
}
