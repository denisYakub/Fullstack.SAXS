using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Models
{
    public record CreateDensityGraphModel(
        Guid UserId,
        Guid AreaId,
        int LayersNum
    );
}
