using System.Text.Json;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Contracts;

namespace Fullstack.SAXS.Application.Services
{
    public class SpService(IStorage storage, IFileService file) : ISpService
    {
        public async Task<string> GetAsync(Guid id)
        {
            var area = await storage.GetAreaAsync(id);

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = true,
            };

            var json = JsonSerializer.Serialize(area, options);

            return json;
        }

        public async Task<string> GetAllAsync(Guid? userId = null)
        {
            if (userId.HasValue)
            {
                return await storage.GetGenerationsAsync(userId.Value);
            }
            else
            {
                return await storage.GetAllGenerationsAsync();
            }
        }

        public async Task<byte[]> GetAtomsAsync(Guid id)
        {
            var area = await storage.GetAreaAsync(id);

            return file.GetCSVAtoms(area);
        }
    }
}
