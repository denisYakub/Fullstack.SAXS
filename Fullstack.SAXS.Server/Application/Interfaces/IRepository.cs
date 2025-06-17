namespace Fullstack.SAXS.Server.Application.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        Task AddAsync(T entity);
        void Save(Guid idUser);
        Task SaveAsync(Guid idUser);
    }
}
