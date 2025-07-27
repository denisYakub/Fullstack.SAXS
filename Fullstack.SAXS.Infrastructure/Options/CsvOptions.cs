using System.ComponentModel.DataAnnotations;

namespace Fullstack.SAXS.Infrastructure.Options
{
    public class CsvOptions
    {
        [Required]
        public required string FolderPath { get; set; }
    }
}
