namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IGraphService
    {
        Task<string> GetHtmlPageAsync(double[] x, double[] y, string xLable, string yLable, string title);
    }
}
