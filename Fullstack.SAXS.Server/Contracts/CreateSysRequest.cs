using System.ComponentModel.DataAnnotations;
using Fullstack.SAXS.Domain.Commands;
using MathNet.Numerics.Distributions;

namespace Fullstack.SAXS.Server.Contracts
{
    public record CreateSysRequest(
        double? AreaRadius, double? Nc,
        double ParticleMinSize, double ParticleMaxSize,
        double ParticleAlphaRotation,
        double ParticleBetaRotation,
        double ParticleGammaRotation,
        double ParticleSizeShape, double ParticleSizeScale,
        int ParticleNumber, int AreaNumber
    ) : IValidatableObject
    {
        public double AreaSize
        {
            get
            {
                // If Area is shpere and particle is icosahedron
                if (Nc.HasValue)
                {
                    var particleConst = (5.0 / 12.0) * (3.0 + Math.Sqrt(5.0));
                    var areaConst = (4.0 / 3.0) * Math.PI;

                    var gamma = new Gamma(ParticleSizeShape, ParticleSizeScale);

                    var particleVolumeSum = 
                        Enumerable
                        .Repeat(
                            particleConst * Math.Pow(
                                gamma.GetGammaRandom(ParticleMinSize, ParticleMaxSize), 
                                3
                            ), 
                            ParticleNumber
                        )
                        .Sum();

                    var R3 = particleVolumeSum / (Nc.Value * areaConst);

                    return Math.Pow(R3, 1.0 / 3.0);
                }

                if (AreaRadius.HasValue)
                {
                    return AreaRadius.Value;
                }

                throw new InvalidOperationException("Both AreaRadius and Nc are null.");
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!AreaRadius.HasValue && !Nc.HasValue)
                yield return new ValidationResult("Either AreaRadius or Nc must be specified.");

            if (ParticleMinSize > ParticleMaxSize)
                yield return new ValidationResult("ParticleMinSize must be <= ParticleMaxSize.");

            if (AreaNumber <= 0 || ParticleNumber <= 0)
                yield return new ValidationResult("AreaNumber and ParticleNumber must be > 0.");
        }
    }
}
