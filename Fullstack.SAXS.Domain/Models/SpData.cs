using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fullstack.SAXS.Domain.Models
{
    [Table("sp_data")]
    public class SpData
    {
        [Key]
        [Column("id_data")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("path")]
        public string Path { get; set; } = string.Empty;

        [Column("generation_id")]
        public Guid GenId { get; set; }

        [ForeignKey(nameof(GenId))]
        public SpGeneration? Gen { get; set; }
    }
}
