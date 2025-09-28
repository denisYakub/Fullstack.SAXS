using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Domain.Abstractions.Factories
{
    public abstract class AreaFactory
    {
        public abstract IEnumerable<Area> GetAreas(AreaCreateDTO dto);
    }
}
