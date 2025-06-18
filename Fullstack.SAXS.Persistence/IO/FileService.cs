using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Server.Domain.Entities.Areas;

namespace Fullstack.SAXS.Infrastructure.IO
{
    public class FileService : IFileService
    {
        public Area Read(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<Area> ReadAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public string Write(Area obj, string folderPath)
        {
            throw new NotImplementedException();
        }

        public Task<string> WriteAsync(Area obj, string folderPath)
        {
            throw new NotImplementedException();
        }
    }
}
