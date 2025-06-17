using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fullstack.SAXS.Server.Domain.Entities.Sp
{
    [Table("sp_data")]
    public class SpData
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; }
        [Column("path")]
        public string Path { get; private set; }
        public SpGeneration SpGeneration { get; private set; }

        public SpData()
        {
            Id = Guid.Empty;
            Path = string.Empty;
        }

        public SpData(string path)
        {
            Id = Guid.NewGuid();
            Path = path;
        }

        public SpData(Guid id, string path)
        {
            Id = id;
            Path = path;
        }
    }
}
