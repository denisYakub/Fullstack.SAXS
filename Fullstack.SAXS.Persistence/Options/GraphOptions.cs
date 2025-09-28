using System.ComponentModel.DataAnnotations;

namespace Fullstack.SAXS.Persistence.Options
{
    internal class GraphOptions
    {
        [Required]
        public required Uri Uri { get; set; }
        [Required]
        public required string RunFilePath { get; set; }
    }
}
