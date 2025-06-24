using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fullstack.SAXS.Domain.Entities.Sp
{
    [Table("sp_generation_number_counter")]
    public class SpGenerationNumberCounter
    {
        [Key]
        [Column("id")]
        public int Id { get; private set; }
        [Column("generation_current_number")]
        public long CurrentNum { get; private set; }

        public SpGenerationNumberCounter()
        {
            Id = 1;
            CurrentNum = 0;
        }

        public SpGenerationNumberCounter(int id, long currentNum)
        {
            Id=id;
            CurrentNum = currentNum;
        }

        public void Increase()
        {
            CurrentNum += 1;
        }
    }
}
