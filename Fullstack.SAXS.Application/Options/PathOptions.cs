using System.ComponentModel.DataAnnotations;

namespace Fullstack.SAXS.Application.Options
{
    public class PathOptions
    {
        [Required]
        public required string CsvFolder { get; set; }
        [Required]
        public required Uri GraphUriPath { get; set; }
        [Required]
        public required string GraphServerFilePath { get; set; }
        [Required]
        public required Uri KafkaUriPath { get; set; }
    }
}
