using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Domain.Abstractions.Services
{
    public interface ISysService
    {
        IReadOnlyCollection<Area> Create(AreaCreateDTO areDTO, ParticleCreateDTO particleDTO);
    }
}
