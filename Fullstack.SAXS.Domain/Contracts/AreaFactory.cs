using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Contracts
{
    public abstract class AreaFactory
    {
        public abstract IEnumerable<Area> GetAreas(double areaSize, int number, double maxParticleSize);
    }
}
