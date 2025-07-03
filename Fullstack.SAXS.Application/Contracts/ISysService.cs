using Fullstack.SAXS.Domain.Commands;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Enums;

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
        Task<string> CreateIntensOptGrafAsync(
            Guid id,
            double QMin, double QMax, int QNum, StepTypes StepType
        );
        Task<string> CreatePhiGrafAsync(string? userId, Guid id, int layersNum);
        public static double[] CreateQs(double QMin, double QMax, int QNum, StepTypes StepType = StepTypes.Linear)
        {
            double[] result = new double[QNum];

            switch (StepType)
            {
                case StepTypes.Linear:
                    for (int i = 0; i < QNum; i++)
                        result[i] = QMin + i * ((QMax - QMin) / QNum - 1);
                    break;
                case StepTypes.Logarithmic:
                    for (int i = 0; i < QNum; i++)
                    {
                        var logMin = Math.Log(QMin);
                        var logMax = Math.Log(QMax);
                        var t = (double)i / (QNum - 1);
                        result[i] = Math.Exp(logMin + t * (logMax - logMin));
                    }
                    break;
            }

            return result;
        }
    }
}
