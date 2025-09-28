using Fullstack.SAXS.Domain.Abstractions.Factories;
using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Infrastructure.Factories
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
