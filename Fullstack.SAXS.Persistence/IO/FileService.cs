using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.ValueObjects;
using System.Collections.Generic;
using System.Text;

namespace Fullstack.SAXS.Persistence.IO
{
    public class FileService(IStringService @string) : IFileService
    {
        public async Task<Area> ReadAsync(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            var areaParameters = fileName
                .Split('_')
                .Select(s => s.Split('#'))
                .Where(keyValue => keyValue.Length == 2)
                .ToDictionary(keyValue => keyValue[0], keyValue => keyValue[1]);

            var series = int.Parse(
                areaParameters.GetValueOrDefault("Series") ??
                throw new FormatException("Missing Series")
            );

            var outerR = double.Parse(
                areaParameters.GetValueOrDefault("OuterRadius") ??
                throw new FormatException("Missing OuterRadius")
            );

            var areaType = Enum.Parse<AreaTypes>(areaParameters.GetValueOrDefault("AreaType") ?? 
                throw new FormatException("Missing AreaType")
            );

            var particleType = Enum.Parse<ParticleTypes>(areaParameters.GetValueOrDefault("ParticlesType") ??
                throw new FormatException("Missing ParticlesType")
            );

            List<Particle> particles = [];

            await foreach ( var line in File.ReadLinesAsync(filePath, Encoding.UTF8))
            {
                var data = line.Split(';');

                if (data.Length < 3)
                    throw new FormatException("Need 3 element to create particle");

                var size = double.Parse(data[0]);
                var center = Vector3D.Parse(data[1]);
                var angles = EulerAngles.Parse(data[2]);

                Particle particle = particleType switch
                {
                    ParticleTypes.Icosahedron => new IcosahedronParticle(series, center, angles),
                    ParticleTypes.C60 => new C60(series, center, angles),
                    ParticleTypes.C70 => new C70(series, center, angles),
                    ParticleTypes.C240 => new C240(series, center, angles),
                    ParticleTypes.C540 => new C540(series, center, angles),
                    _ => throw new NotImplementedException()
                };

                particles.Add(particle);
            }

            return new SphereArea(series, outerR, particles);
        }

        public async Task<string> WriteAsync(Area obj, long generationNum)
        {
            if (obj.Particles is null)
                throw new FormatException("Area contains no particles");

            var fileName = string.Join('_', new[] 
            {
                $"{nameof(obj.Series)}#{obj.Series}",
                $"{nameof(obj.OuterRadius)}#{obj.OuterRadius}",
                $"{nameof(obj.AreaType)}#{obj.AreaType}",
                $"{nameof(obj.ParticlesType)}#{obj.ParticlesType}",
            }) + ".csv";

            var folder = Path.Combine(@string.GetCsvFolder(), $"Generation_{generationNum}");
            Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, fileName);

            await using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

            foreach (var p in obj.Particles)
            {
                await writer.WriteLineAsync($"{p.Size};{p.Center};{p.RotationAngles}");
            }

            return filePath;
        }

        public byte[] GetCSVAtoms(Area area)
        {
            var csvLines = new List<string>();

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
