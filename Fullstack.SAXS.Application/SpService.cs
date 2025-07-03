using System.Text.Json;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Contracts;

namespace Fullstack.SAXS.Application
{
    public class SpService(IStorage storage, IFileService file) : ISpService
    {
        public string Get(Guid id)
        {
            var area = storage.GetArea(id);

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = true,
            };

            var json = JsonSerializer.Serialize(area, options);

            return json;
        }

        public byte[] GetAtoms(Guid id)
        {
            var area = storage.GetArea(id);

            return file.GetCSVAtoms(area);
        }

        public string GetAll(string userId = null)
        {
            string result;

            if (userId == null)
            {
                result = storage.GetAllGenerations();
            }
            else
            {
                var idUser = Guid.Parse(userId);

                result = storage.GetGenerations(idUser);
            }

            return result;
        }
    }
}
