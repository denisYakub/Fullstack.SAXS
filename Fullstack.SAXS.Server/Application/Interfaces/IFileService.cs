namespace Fullstack.SAXS.Server.Application.Interfaces
{
    public interface IFileService<T>
    {
        string Write(T obj, string folderPath);
        T Read(string filePath);
        Task<string> WriteAsync(T obj, string folderPath);
        Task<T> ReadAsync(string filePath);
    }
}
