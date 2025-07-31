using Fullstack.SAXS.Domain.Models;

namespace Fullstack.SAXS.Application.Contracts
{
    public interface ISysService
    {
        Task CreateAsync(CreateSysModel data);
        Task<string> CreateIntensOptGraphAsync(CreateIntensityGraphModel model);
        Task<string> CreatePhiGraphAsync(CreateDensityGraphModel model);
    }
}
