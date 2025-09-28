using System.ComponentModel.DataAnnotations;

namespace Fullstack.SAXS.Persistence.Options
{
    internal class CsvOptions
    {
        [Required]
        public required string FolderPath { get; set; }
    }
}
