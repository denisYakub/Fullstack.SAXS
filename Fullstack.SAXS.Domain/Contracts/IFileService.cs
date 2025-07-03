using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IFileService
    {
        string Write(Area obj, long GenerationNum);
        Area Read(string filePath);
        Task<string> WriteAsync(Area obj, long GenerationNum);
        Task<Area> ReadAsync(string filePath);
        byte[] GetCSVAtoms(Area area);
    }
}
