using Fullstack.SAXS.Server.Domain.Entities.Areas;

namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IFileService
    {
        string Write(Area obj, string folderPath);
        Area Read(string filePath);
        Task<string> WriteAsync(Area obj, string folderPath);
        Task<Area> ReadAsync(string filePath);
    }
}
