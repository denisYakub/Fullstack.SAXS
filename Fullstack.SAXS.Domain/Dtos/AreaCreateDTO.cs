using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Dtos
{
    public record AreaCreateDTO(
        double AreaSize,
        int AreaNumber,
        AreaTypes AreaType
    );
}
