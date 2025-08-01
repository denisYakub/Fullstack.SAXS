using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Application.Contracts
{
    public interface IStorage
    {
        Task<Area> GetAreaAsync(Guid idArea);
        Task<string> GetGenerationsAsync(Guid? idUser);
        Task AddRangeAsync(IEnumerable<Area> entities, Guid idUser);
        Task UpdateAvgPhiAsync(Guid idUser, Guid idArea, double avgPhi);
        Task SaveSystemTaskAsync(SystemCreateDto dto);
    }
}
