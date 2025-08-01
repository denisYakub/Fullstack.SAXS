using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Dtos
{
    public record IntensityCreateDTO(
        Guid AreaId,
        double QMin, double QMax,
        int QNum, StepTypes StepType
    );
}
