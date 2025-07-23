using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Domain.Contracts
{
    public abstract class AreaFactory
    {
        public abstract IEnumerable<Area> GetAreas(double areaSize, int number);
    }
}
