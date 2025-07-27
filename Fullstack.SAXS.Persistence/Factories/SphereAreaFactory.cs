using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Infrastructure.Factories
{
    public class SphereAreaFactory : AreaFactory
    {
        public override IEnumerable<Area> GetAreas(double areaSize, int number)
        {
            for (int i = 0; i < number; i++) 
                yield return new SphereArea(i, areaSize);
        }
    }
}
