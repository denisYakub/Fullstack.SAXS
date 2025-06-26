namespace Fullstack.SAXS.Server.Contracts
{
    public record struct CreateIntensOptRequest(
        double QMin, double QMax, int QNum
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
