namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IConnectionStrService
    {
        string GetCsvFolder();
        Uri GetPythonServerUri();
        string GetPythonServerExePath();
    }
}
