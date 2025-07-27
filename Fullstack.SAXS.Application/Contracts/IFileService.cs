using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Application.Contracts
{
    public interface IFileService
    {
        byte[] SaveAtoms(Area obj);
        Task<Area> ReadAsync(string filePath);
        Task<string> WriteAsync(Area obj, long GenerationNum);
    }
}
