namespace Fullstack.SAXS.Application.Config
{
    public class PathOptions
    {
        public required string CsvFolder { get; set; }
        public required Uri GraphUriPath { get; set; }
        public required string GraphServerFilePath { get; set; }
    }
}
