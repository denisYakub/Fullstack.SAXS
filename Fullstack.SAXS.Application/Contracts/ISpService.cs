namespace Fullstack.SAXS.Application.Contracts
{
    public interface ISpService
    {
        Task<string> GetAsync(Guid id);
        Task<string> GetAllAsync(Guid? userId = null);
        Task<byte[]> GetAtomsAsync(Guid id);
    }
}
