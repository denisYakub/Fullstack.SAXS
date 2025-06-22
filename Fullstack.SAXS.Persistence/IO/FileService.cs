using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Persistence.Contracts;
using Fullstack.SAXS.Server.Domain.Entities.Areas;
using Fullstack.SAXS.Server.Domain.Entities.Particles;
using Fullstack.SAXS.Server.Domain.Enums;
using Fullstack.SAXS.Server.Domain.ValueObjects;
using System.Numerics;
using System.Text;

namespace Fullstack.SAXS.Persistence.IO
{
    public class FileService(IStringService @string) : IFileService
    {
        public Area Read(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            var areaData = fileName.Split('_');

            int series = 0;
            float r = 0;
            AreaTypes areaType;
            ParticleTypes? particleType = null;

            foreach ( var data in areaData )
            {
                var name = data.Split('#')[0];
                var value = data.Split('#')[1];

                if (name == "Series")
                    series = int.Parse(value);
                else if (name == "OuterRadius")
                    r = float.Parse(value);
                else if (name == "AreaType")
                    areaType = AreaTypes.Sphere;
                else if (name == "ParticlesType")
                    particleType = ParticleTypes.Icosahedron;
            }

            var lines = File.ReadLines(filePath, Encoding.UTF8);

            if (lines.Count() <= 1)
                return new SphereArea(series, r, null);

            List<Particle> particles = new(lines.Count() - 1);

            foreach ( var line in lines.Skip(1) )
            {
                var size = line.Split(';')[0];
                var center = line.Split(';')[1];
                var rotationAngles = line.Split(';')[2];

                var x = center.Split(' ')[0];
                var y = center.Split(' ')[1];
                var z = center.Split(' ')[2];

                var particle = new IcosahedronParticle(
                    float.Parse(size), 
                    new Vector3(
                        float.Parse(x.Substring(1)),
                        float.Parse(y),
                        float.Parse(z.Substring(0, z.Length - 1))
                    ), 
                    EulerAngles.Parse(rotationAngles)
                );

                particles.Add(particle);
            }

            return new SphereArea(series, r, particles);
        }

        public async Task<Area> ReadAsync(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            var areaData = fileName.Split('_');

            int series = 0;
            float r = 0;
            AreaTypes areaType;
            ParticleTypes? particleType = null;

            foreach (var data in areaData)
            {
                var name = data.Split('#')[0];
                var value = data.Split('#')[1];

                if (name == "Series")
                    series = int.Parse(value);
                else if (name == "OuterRadius")
                    r = float.Parse(value);
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

                    var x = center.Split(' ')[0];
                    var y = center.Split(' ')[1];
                    var z = center.Split(' ')[2];

                    var particle = new IcosahedronParticle(
                        float.Parse(size),
                        new Vector3(
                            float.Parse(x.Substring(1)),
                            float.Parse(y),
                            float.Parse(z.Substring(0, z.Length - 1))
                        ),
                        EulerAngles.Parse(rotationAngles)
                    );

                    particles.Add(particle);
                }

            if (particleType == null)
                return new SphereArea(series, r, null);
            else
                return new SphereArea(series, r, particles);
        }

        public string Write(Area obj)
        {
            var file = new StringBuilder();

            file.AppendJoin('_', [
                $"{nameof(obj.Series)}#{obj.Series}",
                $"{nameof(obj.OuterRadius)}#{obj.OuterRadius}",
                $"{nameof(obj.AreaType)}#{obj.AreaType}",
                $"{nameof(obj.ParticlesType)}#{obj.ParticlesType}",
            ]);
            file.Append(".csv");

            var path = Path.Combine(@string.GetCsvFolder(), file.ToString());

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

        public async Task<string> WriteAsync(Area obj)
        {
            var file = new StringBuilder();

            file.AppendJoin('_', [
                $"{nameof(obj.Series)}#{obj.Series}",
                $"{nameof(obj.OuterRadius)}#{obj.OuterRadius}",
                $"{nameof(obj.AreaType)}#{obj.AreaType}",
                $"{nameof(obj.ParticlesType)}#{obj.ParticlesType}",
            ]);
            file.Append(".csv");

            var path = Path.Combine(@string.GetCsvFolder(), file.ToString());

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
