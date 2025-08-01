using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fullstack.SAXS.Domain.Models
{
    [Table("sp_generation_number_counter")]
    public class SpGenerationNumberCounter(int id, long currentNum)
    {
        [Key]
        [Column("id")]
        public int Id { get; private set; } = id;
        [Column("generation_current_number")]
        public long CurrentNum { get; private set; } = currentNum;

        public SpGenerationNumberCounter()
            : this(1, 0) { }

        public void Increase() =>
            CurrentNum += 1;
    }
}
