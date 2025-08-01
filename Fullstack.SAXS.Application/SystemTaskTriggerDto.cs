using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fullstack.SAXS.Application
{
    internal class SystemTaskTriggerDto
    {
        [JsonPropertyName("id_system_task")]
        public Guid Id { get; set; }

        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }

        [JsonPropertyName("area_size")]
        public double AreaSize { get; set; }

        [JsonPropertyName("area_copies_number")]
        public int AreaNumber { get; set; }

        [JsonPropertyName("particle_number")]
        public int ParticleNumber { get; set; }

        [JsonPropertyName("particle_type")]
        public string ParticleType { get; set; } = string.Empty;

        [JsonPropertyName("particle_min_size")]
        public double ParticleMinSize { get; set; }

        [JsonPropertyName("particle_max_size")]
        public double ParticleMaxSize { get; set; }

        [JsonPropertyName("particle_size_shape")]
        public double ParticleSizeShape { get; set; }

        [JsonPropertyName("particle_size_scale")]
        public double ParticleSizeScale { get; set; }

        [JsonPropertyName("particle_alpha_rotation")]
        public double ParticleAlphaRotation { get; set; }

        [JsonPropertyName("particle_beta_rotation")]
        public double ParticleBetaRotation { get; set; }

        [JsonPropertyName("particles_gamma_rotation")]
        public double ParticleGammaRotation { get; set; }
    }
}
