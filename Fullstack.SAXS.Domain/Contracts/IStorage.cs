using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IStorage
    {
        Task AddRangeAsync(IEnumerable<Area> entities, Guid idUser);
        Task<Area> GetAreaAsync(Guid id);
        Task SaveAvgPhiAsync(Guid idUser, Guid id, double phi);
        Task<string> GetAllGenerationsAsync();
        Task<string> GetGenerationsAsync(Guid idUser);
    }
}
