using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.Extensions;
using Fullstack.SAXS.Domain.Models;
using MathNet.Numerics.Distributions;

namespace Fullstack.SAXS.Persistence.Factories
{
    public class IcosahedronParticleFactory : ParticleFactory
    {
        public override ParticleTypes Type => 
            ParticleTypes.Icosahedron;

        public override IEnumerable<Particle> GetParticlesInf(CreateParticelData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Shouldn't be null.");

            var random = new Random();
            var gamma = new Gamma(data.SizeShape, data.SizeScale);

            while (true)
            {
                var size = gamma.GetGammaRandom(data.MinSize, data.MaxSize);

                var a = random.GetEvenlyRandom(-data.AlphaRotation, data.AlphaRotation);
                var b = random.GetEvenlyRandom(-data.BetaRotation, data.BetaRotation);
                var g = random.GetEvenlyRandom(-data.GammaRotation, data.GammaRotation);

                var x = random.GetEvenlyRandom(data.MinX, data.MaxX);
                var y = random.GetEvenlyRandom(data.MinY, data.MaxY);
                var z = random.GetEvenlyRandom(data.MinZ, data.MaxZ);

                yield return new IcosahedronParticle(size, new(x, y, z), new(a, b, g));
            }
        }
    }
}
