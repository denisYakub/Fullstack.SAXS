namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IStringService
    {
        string GetCsvFolder();
        string GetGraphUriPath();
        string GetPythonServerFilePath();
    }
}
