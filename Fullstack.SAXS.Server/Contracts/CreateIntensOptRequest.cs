using System.ComponentModel.DataAnnotations;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Server.Contracts
{
    public record CreateIntensOptRequest(
        double QMin, double QMax, int QNum, StepTypes StepType
    ) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (QMin <= 0 && QMax <= 0)
                yield return new ValidationResult("QMin and QMax should be greater then 0.");

            if (QMin > QMax)
                yield return new ValidationResult("QMin must be smaller then QMax.");
        }
    }
}
