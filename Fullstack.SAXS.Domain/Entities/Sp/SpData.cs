using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fullstack.SAXS.Domain.Entities.Sp
{
    [Table("sp_data")]
    public class SpData(Guid id, string path)
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; } = id;
        [Column("path")]
        public string Path { get; private set; } = path;

        public SpData()
            : this(Guid.Empty, string.Empty)
        { }

        public SpData(string path) 
            : this(Guid.NewGuid(), path) 
        { }
    }
}
