namespace Fullstack.SAXS.Server.Contracts
{
    public record struct CreateSysRequest(
        float? AreaRadius, float? Nc,
        float ParticleMinSize, float ParticleMaxSize,
        float ParticleAlphaRotation,
        float ParticleBetaRotation,
        float ParticleGammaRotation,
        float ParticleSizeShape, float ParticleSizeScale,
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

        public readonly float AreaSize
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
