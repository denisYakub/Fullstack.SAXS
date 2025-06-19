using Fullstack.SAXS.Infrastructure.IO;
using Fullstack.SAXS.Server.Domain.Entities.Areas;
using Fullstack.SAXS.Server.Domain.Entities.Particles;

namespace Fullstack.SAXS.Persistence.Tests
{
    public class Tests
    {
        private FileService _fileService;
        private string _folderPath;

        [SetUp]
        public void Setup()
        {
            _fileService = new FileService();
            _folderPath = 
                Path
                .Combine(
                    AppContext.BaseDirectory,
                    "FileServiceTests"
                );

            if (!Directory.Exists(_folderPath))
                Directory.CreateDirectory(_folderPath);
        }

        [Test]
        public void Test_Write_Method()
        {
            //Init
            var areaWithoutParticles = new SphereArea(0, 100, 5);
            var areaWithParticles = new SphereArea(1, 100, 5);

            Particle[] particles = [
                new IcosahedronParticle(5, new (0, 0, 0), new (360, 360, 360)),
                new IcosahedronParticle(3, new (10, 10, 10), new (360, 360, 360)),
                new IcosahedronParticle(1, new (-10, -10, -10), new (360, 360, 360)),
            ];
            //Do
            areaWithParticles.Fill(particles, 3);

            var p = areaWithParticles.ParticlesType;

            var resultFile1 = _fileService.Write(areaWithoutParticles, _folderPath);
            var resultFile2 = _fileService.Write(areaWithParticles, _folderPath);
            //Assert
            Assert.Pass(resultFile1);
            Assert.Pass(resultFile2);
        }
    }
}
