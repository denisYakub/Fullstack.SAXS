using Fullstack.SAXS.Server.Domain.Entities.Areas;

namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IStorage
    {
        void Add(Area entity);
        Task AddAsync(Area entity);
        void Save(Guid idUser);
        Task SaveAsync(Guid idUser);
    }
}
