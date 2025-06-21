using Fullstack.SAXS.Infrastructure.IO;
using Fullstack.SAXS.Persistence.Tests.Mocks;
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
            var strService = new StringServiceMock();
            _fileService = new FileService(strService);
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

            var resultFile1 = _fileService.Write(areaWithoutParticles);
            var resultFile2 = _fileService.Write(areaWithParticles);
            //Assert
            Assert.Pass(resultFile1);
            Assert.Pass(resultFile2);
        }

        [Test]
        public void Test_Read_Method()
        {
            //Init
            var path = @"C:\Users\denis\source\repos\Fullstack.SAXS\Fullstack.SAXS.Persistence.Tests\bin\Debug\net9.0\FileServiceTests\Series#1_OuterRadius#100_AreaType#Sphere_ParticlesType#Icosahedron.csv";
            //Do
            var area = _fileService.Read(path);
            //Assert
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Particles.Count(), Is.EqualTo(3));
        }
    }
}
