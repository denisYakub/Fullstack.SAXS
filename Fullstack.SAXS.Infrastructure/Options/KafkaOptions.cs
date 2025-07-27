using System.ComponentModel.DataAnnotations;

namespace Fullstack.SAXS.Infrastructure.Options
{
    public class KafkaOptions
    {
        [Required]
        public required string Uri { get; set; }
        [Required]
        public required string Topic { get; set; }
    }
}
