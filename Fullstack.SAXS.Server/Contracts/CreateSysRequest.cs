using System.ComponentModel.DataAnnotations;
using Fullstack.SAXS.Domain.Commands;
using Fullstack.SAXS.Domain.Enums;
using MathNet.Numerics.Distributions;

namespace Fullstack.SAXS.Server.Contracts
{
    public record CreateSysRequest(
        double? AreaRadius, double? Nc, ParticleTypes ParticleType,
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
                if (Nc.HasValue)
                {
                    var sphereConst = (4.0 / 3.0) * Math.PI;

                    var gamma = new Gamma(ParticleSizeShape, ParticleSizeScale);

                    var particleVolumeSum = 
                        Enumerable
                        .Repeat(
                            sphereConst * Math.Pow(
                                gamma.GetGammaRandom(ParticleMinSize, ParticleMaxSize), 
                                3
                            ), 
                            ParticleNumber
                        )
                        .Sum();

                    var R3 = particleVolumeSum / (sphereConst * Nc.Value);

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
