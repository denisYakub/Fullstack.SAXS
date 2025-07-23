namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IConnectionStrService
    {
        string GetStoragePath();
        Uri GetGraphServerUri();
        string GetGraphServerFilePath();
    }
}
