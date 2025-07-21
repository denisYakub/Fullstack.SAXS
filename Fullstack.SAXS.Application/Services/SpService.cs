using System.Text.Json;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Contracts;

namespace Fullstack.SAXS.Application.Services
{
    public class SpService(IStorage storage, IFileService file) : ISpService
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true,
        };

        public async Task<string> GetAsync(Guid id)
        {
            var area = await storage
                .GetAreaAsync(id)
                .ConfigureAwait(false);

            var json = JsonSerializer.Serialize(area, _jsonOptions);

            return json;
        }

        public async Task<string> GetAllAsync(Guid? userId = null)
        {
            if (userId.HasValue)
            {
                return await storage
                    .GetGenerationsAsync(userId.Value)
                    .ConfigureAwait(false);
            }
            else
            {
                return await storage
                    .GetAllGenerationsAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<byte[]> GetAtomsAsync(Guid id)
        {
            var area = await storage
                .GetAreaAsync(id)
                .ConfigureAwait(false);

            return file.GetCSVAtoms(area);
        }
    }
}
