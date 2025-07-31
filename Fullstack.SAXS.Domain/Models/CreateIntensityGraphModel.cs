using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Models
{
    public record CreateIntensityGraphModel(
        Guid AreaId,
        double QMin, double QMax,
        int QNum, StepTypes StepType
    );
}
