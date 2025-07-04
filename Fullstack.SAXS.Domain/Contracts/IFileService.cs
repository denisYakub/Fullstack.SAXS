using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IFileService
    {
        Task<string> WriteAsync(Area obj, long GenerationNum);
        Task<Area> ReadAsync(string filePath);
        byte[] GetCSVAtoms(Area area);
    }
}
