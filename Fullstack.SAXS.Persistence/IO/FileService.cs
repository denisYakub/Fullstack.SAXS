using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.ValueObjects;
using System.Collections.Generic;
using System.Globalization;
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

            if (!areaParameters.TryGetValue("Series", out var seriesStr) || !int.TryParse(seriesStr, out var series))
                throw new FormatException("Missing or invalid value for Series.");

            if (!areaParameters.TryGetValue("OuterRadius", out var outerRStr) || !double.TryParse(outerRStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var outerR))
                throw new FormatException("Missing or invalid value for OuterRadius.");

            if (!areaParameters.TryGetValue("AreaType", out var areaTypeStr) || !Enum.TryParse(areaTypeStr, out AreaTypes areaType))
                throw new FormatException("Missing or invalid value for AreaType.");

            if (!areaParameters.TryGetValue("ParticlesType", out var particleTypeStr) || !Enum.TryParse(particleTypeStr, out ParticleTypes particleType))
                throw new FormatException("Missing or invalid value for ParticlesType.");

            List<Particle> particles = [];

            await foreach ( var line in File.ReadLinesAsync(filePath, Encoding.UTF8))
            {
                var data = line.Split(';');

                if (data.Length < 3)
                    throw new FormatException("Each line must contain at least 3 values: size;position;rotation.");

                if (!double.TryParse(data[0], NumberStyles.Float, CultureInfo.GetCultureInfo("ru-RU"), out var size))
                    throw new FormatException($"Invalid size value: '{data[0]}'");

                var center = Vector3D.Parse(data[1]);
                var angles = EulerAngles.Parse(data[2]);

                Particle particle = particleType switch
                {
                    ParticleTypes.Icosahedron => new IcosahedronParticle(size, center, angles),
                    ParticleTypes.C60 => new C60(size, center, angles),
                    ParticleTypes.C70 => new C70(size, center, angles),
                    ParticleTypes.C240 => new C240(size, center, angles),
                    ParticleTypes.C540 => new C540(size, center, angles),
                    _ => throw new NotSupportedException($"Particle type '{particleType}' is not supported.")
                };

                particles.Add(particle);
            }

            return new SphereArea(series, outerR, particles);
        }

        public async Task<string> WriteAsync(Area obj, long generationNum)
        {
            if (obj.Particles is null || obj.Particles.Count() == 0)
                throw new FormatException("Area contains no particles.");

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
                var size = p.Size.ToString(CultureInfo.GetCultureInfo("ru-RU"));
                var center = p.Center.ToString();
                var angles = p.RotationAngles.ToString();

                await writer.WriteLineAsync($"{size};{center};{angles}");
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
