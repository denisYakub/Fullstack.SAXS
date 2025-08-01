using System.ComponentModel.DataAnnotations;

namespace Fullstack.SAXS.Infrastructure.Options
{
    internal class CsvOptions
    {
        [Required]
        public required string FolderPath { get; set; }
    }
}
