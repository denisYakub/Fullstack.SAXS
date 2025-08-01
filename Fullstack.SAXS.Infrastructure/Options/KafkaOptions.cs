using System.ComponentModel.DataAnnotations;

namespace Fullstack.SAXS.Infrastructure.Options
{
    internal class KafkaOptions
    {
        [Required]
        public required string Uri { get; set; }
        [Required]
        public required string Topic { get; set; }
        [Required]
        public required string GroupId { get; set; }
    }
}
