using Fullstack.SAXS.Domain.Models;

namespace Fullstack.SAXS.Application.Contracts
{
    public interface ISysService
    {
        Task CreateAsync(Guid userId, CreateSysData data);
        Task<string> CreateIntensOptGraphAsync(Guid id, CreateQIData data);
        Task<string> CreatePhiGraphAsync(Guid userId, Guid id, int layersNum);
    }
}
