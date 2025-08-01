using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.Extensions;
using MathNet.Numerics.Distributions;

namespace Fullstack.SAXS.Persistence.Factories
{
    public class C540Factory : ParticleFactory
    {
        public override ParticleTypes Type => ParticleTypes.C540;

        public override IEnumerable<Particle> GetParticlesInf(ParticleCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Shouldn't be null.");

            var random = new Random();
            var gamma = new Gamma(dto.SizeShape, dto.SizeScale);

            while (true)
            {
                var size = gamma.GetGammaRandom(dto.MinSize, dto.MaxSize);

                var a = random.GetEvenlyRandom(-dto.AlphaRotation, dto.AlphaRotation);
                var b = random.GetEvenlyRandom(-dto.BetaRotation, dto.BetaRotation);
                var g = random.GetEvenlyRandom(-dto.GammaRotation, dto.GammaRotation);

                var x = random.GetEvenlyRandom(dto.MinX, dto.MaxX);
                var y = random.GetEvenlyRandom(dto.MinY, dto.MaxY);
                var z = random.GetEvenlyRandom(dto.MinZ, dto.MaxZ);

                yield return new C540(size, new(x, y, z), new(a, b, g));
            }
        }
    }
}
