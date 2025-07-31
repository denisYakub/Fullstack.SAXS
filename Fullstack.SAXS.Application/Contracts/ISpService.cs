using Fullstack.SAXS.Domain.Models;

namespace Fullstack.SAXS.Application.Contracts
{
    public interface ISpService
    {
        Task<string> GetAsync(Guid idArea);
        Task<string> GetAllAsync(GetGenerationsModel model);
        Task<byte[]> GetAtomsAsync(Guid idArea);
    }
}
