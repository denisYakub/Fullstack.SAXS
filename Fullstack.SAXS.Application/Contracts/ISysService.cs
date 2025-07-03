namespace Fullstack.SAXS.Application.Contracts
{
    public interface ISysService
    {
        void Create(
            string? userId,
            double AreaSize, int AreaNumber, int ParticleNumber,
            double ParticleMinSize, double ParticleMaxSize,
            double ParticleSizeShape, double ParticleSizeScale,
            double ParticleAlphaRotation,
            double ParticleBetaRotation,
            double ParticleGammaRotation
        );
        string Get(Guid id);
        byte[] GetAtoms(Guid id);
        Task<string> CreateIntensOptGrafAsync(
            string? userId,
            Guid id,
            double QMin, double QMax, int QNum
        );
        Task<string> CreatePhiGrafAsync(string? userId, Guid id, int layersNum);
    }
}
