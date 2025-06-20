using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Server.Domain.Entities.Areas;
using System.Diagnostics;
using System.IO;
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
            var file = new StringBuilder();

            file.AppendJoin('_', [
                $"{nameof(obj.Series)}#{obj.Series}",
                $"{nameof(obj.OuterRadius)}#{obj.OuterRadius}",
                $"{nameof(obj.AreaType)}#{obj.AreaType}",
                $"{nameof(obj.ParticlesType)}#{obj.ParticlesType}",
            ]);
            file.Append(".csv");

            var path = Path.Combine(folderPath, file.ToString());

            var prtcls = obj.Particles;

            var text = new StringBuilder();

            if (prtcls.Any())
            {
                var prtclF = prtcls.First();
                string[] headers = [
                    nameof(prtclF.Size),
                    nameof(prtclF.Center),
                    nameof(prtclF.RotationAngles),
                ];

                text.AppendJoin(";", headers);
                text.AppendLine();

                foreach ( var prtcl in prtcls)
                {
                    string[] values = [
                        prtcl.Size.ToString(),
                        prtcl.Center.ToString(),
                        prtcl.RotationAngles.ToString()
                    ];

                    text.AppendJoin(";", values);
                    text.AppendLine();
                }
            }

            File.WriteAllText(path, text.ToString(), Encoding.UTF8);

            return path;
        }

        public async Task<string> WriteAsync(Area obj, string folderPath)
        {
            var file = new StringBuilder();

            file.AppendJoin('_', [
                $"{nameof(obj.Series)}#{obj.Series}",
                $"{nameof(obj.OuterRadius)}#{obj.OuterRadius}",
                $"{nameof(obj.AreaType)}#{obj.AreaType}",
                $"{nameof(obj.ParticlesType)}#{obj.ParticlesType}",
            ]);
            file.Append(".csv");

            var path = Path.Combine(folderPath, file.ToString());

            var prtcls = obj.Particles;

            var text = new StringBuilder();

            if (prtcls.Any())
            {
                var prtclF = prtcls.First();
                string[] headers = [
                    nameof(prtclF.Size),
                    nameof(prtclF.Center),
                    nameof(prtclF.RotationAngles),
                ];

                text.AppendJoin(";", headers);
                text.AppendLine();

                foreach (var prtcl in prtcls)
                {
                    string[] values = [
                        prtcl.Size.ToString(),
                        prtcl.Center.ToString(),
                        prtcl.RotationAngles.ToString()
                    ];

                    text.AppendJoin(";", values);
                    text.AppendLine();
                }
            }

            var writeTask = File.WriteAllTextAsync(path, text.ToString(), Encoding.UTF8);

            await writeTask;

            return path;
        }
    }
}
