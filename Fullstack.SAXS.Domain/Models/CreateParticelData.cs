namespace Fullstack.SAXS.Domain.Models
{
    public record CreateParticelData(
        double MinSize, double MaxSize,
        double SizeShape, double SizeScale,
        double AlphaRotation, double BetaRotation, double GammaRotation,
        double MinX, double MaxX,
        double MinY, double MaxY,
        double MinZ, double MaxZ
    );
}
