using System.Text.Json.Serialization;

namespace Fullstack.SAXS.Application.Jsons
{
    internal class SystemCreateJSON
    {
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }

        [JsonPropertyName("area_size")]
        public double AreaSize { get; set; }

        [JsonPropertyName("area_copies_number")]
        public int AreaNumber { get; set; }

        [JsonPropertyName("area_type")]
        public string AreaType { get; set; } = string.Empty;

        [JsonPropertyName("particle_number")]
        public int ParticleNumber { get; set; }

        [JsonPropertyName("particle_type")]
        public string ParticleType { get; set; } = string.Empty;

        [JsonPropertyName("particle_min_size")]
        public double MinSize { get; set; }

        [JsonPropertyName("particle_max_size")]
        public double MaxSize { get; set; }

        [JsonPropertyName("particle_size_shape")]
        public double SizeShape { get; set; }

        [JsonPropertyName("particle_size_scale")]
        public double SizeScale { get; set; }

        [JsonPropertyName("particle_alpha_rotation")]
        public double AlphaRotation { get; set; }

        [JsonPropertyName("particle_beta_rotation")]
        public double BetaRotation { get; set; }

        [JsonPropertyName("particles_gamma_rotation")]
        public double GammaRotation { get; set; }
    }
}
