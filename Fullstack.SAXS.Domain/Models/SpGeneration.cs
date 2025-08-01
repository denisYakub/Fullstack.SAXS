using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fullstack.SAXS.Domain.Models
{
    [Table("sp_generation")]
    public class SpGeneration
    {
        [Key]
        [Column("id_generation")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("generation_number")]
        public long GenNum { get; set; }
        [Column("series_number")]
        public int SeriesNum { get; set; }
        [Column("area_type")]
        public string AreaType { get; set; } = string.Empty;

        [Column("particle_type")]
        public string ParticleType { get; set; } = string.Empty;
        [Column("area_outer_radius")]
        public double AreaOuterRadius { get; set; }
        [Column("phi")]
        public double? Phi { get; set; }
        [Column("particle_number")]
        public int ParticleNum { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }
        public SpData? Data { get; set; }
    }
}
