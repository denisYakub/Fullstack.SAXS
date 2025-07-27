using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Application.Options;
using Microsoft.Extensions.Options;

namespace Fullstack.SAXS.Application.Services
{
    public class ConnectionStrService(IOptions<PathOptions> options) : IConnectionStrService
    {
        public Uri KafkaUriPath() => options.Value.KafkaUriPath;

        public string GetStoragePath() => options.Value.CsvFolder;

        public Uri GetGraphServerUri() => options.Value.GraphUriPath;

        public string GetGraphServerFilePath() => options.Value.GraphServerFilePath;
    }
}
