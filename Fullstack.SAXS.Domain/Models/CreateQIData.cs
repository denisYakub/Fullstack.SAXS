using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Models
{
    public record CreateQIData(
        double QMin, double QMax, 
        int QNum, StepTypes StepType
    );
}
