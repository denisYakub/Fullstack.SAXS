using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Persistence.Factories
{
    public class SphereAreaFactory : AreaFactory
    {
        public override IEnumerable<Area> GetAreas(AreaCreateDTO dto)
        {
            for (int i = 0; i < dto.AreaNumber; i++) 
                yield return new SphereArea(i, dto.AreaSize);
        }
    }
}
