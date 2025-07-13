using Fullstack.SAXS.Application.Config;
using Fullstack.SAXS.Domain.Contracts;
using Microsoft.Extensions.Options;

namespace Fullstack.SAXS.Application.Services
{
    public class StringService(IOptions<PathOptions> options) : IStringService
    {
        public string GetCsvFolder() => options.Value.CsvFolder;

        public string GetGraphUriPath() => options.Value.GraphUriPath;

        public string GetPythonServerFilePath() => options.Value.PythonServerFilePath;
    }
}
