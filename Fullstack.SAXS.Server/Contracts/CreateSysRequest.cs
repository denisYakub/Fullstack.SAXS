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
                if (Nc.HasValue)
                    throw new NotImplementedException();

                return AreaRadius.Value;
            }
        }
    }
}
