using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IStorage
    {
        void Add(Area entity);
        Task AddAsync(Area entity);
        void Save(Guid idUser);
        Task SaveAsync(Guid idUser);
        Area GetArea(Guid id);
        Task<Area> GetAreaAsync(Guid id);
    }
}
