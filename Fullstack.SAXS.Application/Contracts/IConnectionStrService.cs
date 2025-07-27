namespace Fullstack.SAXS.Application.Contracts
{
    public interface IConnectionStrService
    {
        Uri KafkaUriPath();
        string GetStoragePath();
        Uri GetGraphServerUri();
        string GetGraphServerFilePath();
    }
}
