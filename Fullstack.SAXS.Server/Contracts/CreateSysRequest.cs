using Fullstack.SAXS.Domain.Commands;
using MathNet.Numerics.Distributions;

namespace Fullstack.SAXS.Server.Contracts
{
    public record struct CreateSysRequest(
        double? AreaRadius, double? Nc,
        double ParticleMinSize, double ParticleMaxSize,
        double ParticleAlphaRotation,
        double ParticleBetaRotation,
        double ParticleGammaRotation,
        double ParticleSizeShape, double ParticleSizeScale,
        int ParticleNumber, int AreaNumber
    )
    {
        public readonly bool IsLegit
        {
            get
            {
                if (!AreaRadius.HasValue && !Nc.HasValue)
                    return false;

                if (ParticleMinSize > ParticleMaxSize)
                    return false;

                if (AreaNumber <= 0 || ParticleNumber <= 0)
                    return false;

                return true;
            }
        }

        public readonly double AreaSize
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
                else if (AreaRadius.HasValue)
                {
                    return AreaRadius.Value;
                }
                else
                {
                    throw new BadHttpRequestException("AreaRadius and Nc can not be null!");
                }
            }
        }
    }
}
