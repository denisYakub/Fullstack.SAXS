using Fullstack.SAXS.Application.Config;
using Fullstack.SAXS.Domain.Contracts;
using Microsoft.Extensions.Options;

namespace Fullstack.SAXS.Application.Services
{
    public class ConnectionStrService(IOptions<PathOptions> options) : IConnectionStrService
    {
        public string GetStoragePath() => options.Value.CsvFolder;

        public Uri GetGraphServerUri() => options.Value.GraphUriPath;

        public string GetGraphServerFilePath() => options.Value.PythonServerFilePath;
    }
}
