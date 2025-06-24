using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IFileService
    {
        string Write(Area obj);
        Area Read(string filePath);
        Task<string> WriteAsync(Area obj);
        Task<Area> ReadAsync(string filePath);
    }
}
