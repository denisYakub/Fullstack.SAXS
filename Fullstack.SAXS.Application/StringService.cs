using Fullstack.SAXS.Domain.Contracts;

namespace Fullstack.SAXS.Application
{
    public class StringService : IStringService
    {
        public string GetCsvFolder()
        {
            var csvFolder = "C:\\Users\\denis\\source\\repos\\denisYakub\\Fullerenes\\Fullerenes.Server\\CsvResults";

            if (!Directory.Exists(csvFolder))
                Directory.CreateDirectory(csvFolder);

            return csvFolder;
        }

        public string GetGraphUriPath()
        {
            return "http://localhost:5001";
        }

        public string GetPythonServerFilePath()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(currentDir)?.FullName;
            return Path.Combine(rootDir!, "Fullstack.SAXS.api", "Fullstack.SAXS.Api.py");
        }
    }
}
