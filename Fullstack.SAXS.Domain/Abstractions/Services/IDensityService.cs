using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Domain.Abstractions.Services
{
    public interface IDensityService
    {
        Task<(double[] x, double[] y)> CreatePhiCoordAsync(Area area, int numberOfLayers = 5, int numberOfPoints = 100_000);
    }
}
