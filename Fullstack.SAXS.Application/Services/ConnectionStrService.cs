using Fullstack.SAXS.Application.Config;
using Fullstack.SAXS.Domain.Contracts;
using Microsoft.Extensions.Options;

namespace Fullstack.SAXS.Application.Services
{
    public class ConnectionStrService(IOptions<PathOptions> options) : IConnectionStrService
    {
        public string GetCsvFolder() => options.Value.CsvFolder;

        public Uri GetPythonServerUri() => options.Value.GraphUriPath;

        public string GetPythonServerExePath() => options.Value.PythonServerFilePath;
    }
}
