using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Models;

namespace Fullstack.SAXS.Application.Contracts
{
    public interface ISysService
    {
        IReadOnlyCollection<Area> Create(AreaCreateDTO areDTO, ParticleCreateDTO particleDTO);
    }
}
