using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Server.Domain.Entities.Areas;
using System.Text;
using System.Text.Json;

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
            var option = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = true,
            };

            using var document = JsonSerializer.SerializeToDocument(obj, option);
            var root = document.RootElement;

            string[] areaHeaders = [
                nameof(obj.Series),
                nameof(obj.OuterRadius),
                nameof(obj.AreaType),
                nameof(obj.ParticlesType)
            ];

            var partsOfFileName = new List<string>(areaHeaders.Length);

            var values = root.EnumerateObject();

            for (int i = 0; i < areaHeaders.Length; i++)
            {
                var value = 
                    values
                    .First(o => areaHeaders[i] == o.Name)
                    .Value.ToString();

                if(!string.IsNullOrEmpty(value))
                    partsOfFileName.Add($"{areaHeaders[i]}_{value}");
            }

            var file = new StringBuilder();

            file.AppendJoin('_', partsOfFileName);
            file.Append(".csv");

            var path = Path.Combine(folderPath, file.ToString());

            var particles = root.GetProperty(nameof(obj.Particles)).EnumerateArray();

            var strBuilder = new StringBuilder();

            if (particles.Any())
            {
                var prtcl = obj.Particles.First();

                string[] headers = [
                    nameof(prtcl.Size),
                    nameof(prtcl.Center),
                    nameof(prtcl.RotationAngles),
                ];

                strBuilder.AppendJoin(',', headers);
                strBuilder.AppendLine();

                var row = new string[headers.Count()];

                foreach ( var particle in particles)
                {
                    foreach ( var field in particle.EnumerateObject())
                    {
                        if (field.Name == headers[0])
                            row[0] = field.Value.ToString();
                        else if (field.Name == headers[1])
                            row[1] = field.Value.ToString();
                        else if (field.Name == headers[2])
                            row[2] = field.Value.ToString();
                    }

                    strBuilder.AppendJoin(',', row);
                    strBuilder.AppendLine();
                }
            }

            File.WriteAllText(path, strBuilder.ToString(), new UTF8Encoding());

            return path;
        }

        public Task<string> WriteAsync(Area obj, string folderPath)
        {
            throw new NotImplementedException();
        }
    }
}
