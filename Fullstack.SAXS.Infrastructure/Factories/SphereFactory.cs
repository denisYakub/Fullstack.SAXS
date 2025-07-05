using Fullstack.SAXS.Domain.Commands;
using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;
using MathNet.Numerics.Distributions;

namespace Fullstack.SAXS.Infrastructure.Factories
{
    public class SphereFactory : AreaFactory
    {
        public override IEnumerable<Area> GetAreas(double areaSize, int number, double maxParticleSize)
        {
            for (int i = 0; i < number; i++) 
                yield return new SphereArea(i, areaSize, maxParticleSize);
        }
    }
}
