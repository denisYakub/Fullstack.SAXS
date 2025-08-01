using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Models
{
    [Table("sp_system_tasks")]
    public class SpSystemTask
    {
        [Key]
        [Column("id_system_task")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("state")]
        public TaskState State { get; set; }

        [Column("area_size")]
        public double AreaSize { get; set; }
        [Column("area_copies_number")]
        public int AreaNumber { get; set; }
        [Column("area_type")]
        public string AreaType { get; set; } = string.Empty;
        [Column("particle_number")]
        public int ParticleNumber { get; set; }
        [Column("particle_type")]
        public string ParticleType { get; set; } = string.Empty;
        [Column("particle_min_size")]
        public double ParticleMinSize { get; set; }
        [Column("particle_max_size")]
        public double ParticleMaxSize { get; set; }
        [Column("particle_size_shape")]
        public double ParticleSizeShape { get; set; }
        [Column("particle_size_scale")]
        public double ParticleSizeScale { get; set; }
        [Column("particle_alpha_rotation")]
        public double ParticleAlphaRotation { get; set; }
        [Column("particle_beta_rotation")]
        public double ParticleBetaRotation { get; set; }
        [Column("particles_gamma_rotation")]
        public double ParticleGammaRotation { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }
    }
}
