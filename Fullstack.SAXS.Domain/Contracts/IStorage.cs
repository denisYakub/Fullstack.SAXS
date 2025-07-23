using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IStorage
    {
        Task<Area> GetAreaAsync(Guid idArea);
        Task<string> GetAllGenerationsAsync();
        Task<string> GetAllGenerationsAsync(Guid idUser);
        Task AddRangeAsync(IEnumerable<Area> entities, Guid idUser);
        Task UpdateAvgPhiAsync(Guid idUser, Guid idArea, double avgPhi);
    }
}
