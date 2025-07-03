using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Server.Contracts
{
    public record struct CreateIntensOptRequest(
        double QMin, double QMax, int QNum, StepTypes StepType
    )
    {
        public readonly bool isLegit
        {
            get
            {
                if (QMin > QMax)
                    return false;

                if (QNum <= 0)
                    return false;

                return true;
            }
        }
    }
}
