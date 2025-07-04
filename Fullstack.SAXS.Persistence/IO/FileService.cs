using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.ValueObjects;
using System.Text;

namespace Fullstack.SAXS.Persistence.IO
{
    public class FileService(IStringService @string) : IFileService
    {
        public async Task<Area> ReadAsync(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            var areaData = fileName.Split('_');

            int series = 0;
            var r = 0.0;
            AreaTypes areaType;
            ParticleTypes? particleType = null;

            foreach (var data in areaData)
            {
                var name = data.Split('#')[0];
                var value = data.Split('#')[1];

                if (name == "Series")
                    series = int.Parse(value);
                else if (name == "OuterRadius")
                    r = double.Parse(value);
                else if (name == "AreaType")
                    areaType = AreaTypes.Sphere;
                else if (name == "ParticlesType")
                    particleType = ParticleTypes.Icosahedron;
            }

            var lines = File.ReadLinesAsync(filePath, Encoding.UTF8).GetAsyncEnumerator();

            List<Particle> particles = [];

            if (await lines.MoveNextAsync())
                while (await lines.MoveNextAsync())
                {
                    var data = lines.Current.Split(';');

                    var size = data[0];
                    var center = data[1];
                    var rotationAngles = data[2];

                    var particle = new IcosahedronParticle(
                        double.Parse(size),
                        Vector3D.Parse(center),
                        EulerAngles.Parse(rotationAngles)
                    );

                    particles.Add(particle);
                }

            if (particleType == null)
                return new SphereArea(series, r, null);
            else
                return new SphereArea(series, r, particles);
        }

        public async Task<string> WriteAsync(Area obj, long GenerationNum)
        {
            var file = new StringBuilder();

            file.AppendJoin('_', [
                $"{nameof(obj.Series)}#{obj.Series}",
                $"{nameof(obj.OuterRadius)}#{obj.OuterRadius}",
                $"{nameof(obj.AreaType)}#{obj.AreaType}",
                $"{nameof(obj.ParticlesType)}#{obj.ParticlesType}",
            ]);
            file.Append(".csv");

            var mainFolder = @string.GetCsvFolder();
            var subFolder = $"Generation_{GenerationNum}";

            var folder = Path.Combine(mainFolder, subFolder);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, file.ToString());

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

        public byte[] GetCSVAtoms(Area area)
        {
            var csvLines = new List<string>() { "X;Y;Z" };

            csvLines.AddRange(
                area
                .Particles
                .SelectMany(p => p.Vertices)
                .Select(v => $"{v.X};{v.Y};{v.Z}")
            );

            var csvContent = string.Join("\n", csvLines);

            var bytes = Encoding.UTF8.GetBytes(csvContent);

            return bytes;
        }
    }
}
