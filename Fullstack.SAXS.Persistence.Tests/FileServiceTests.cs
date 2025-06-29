using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Persistence.IO;
using Moq;

namespace Fullstack.SAXS.Persistence.Tests
{
    public class FileServiceTests
    {
        private FileService _fileService;
        private string _areaWithParticlesPath;
        private string _areaWithoutParticlesPath;

        [SetUp]
        public void Setup()
        {
            var strServiceMock = new Mock<IStringService>();

            var folder = Path.Combine(
                AppContext.BaseDirectory,
                "FilesForTests"
            );

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            strServiceMock
                .Setup(s => s.GetCsvFolder())
                .Returns(folder);

            _fileService = new FileService(strServiceMock.Object);

            _areaWithoutParticlesPath = Path.Combine(
                folder, 
                "Series#0_OuterRadius#100_AreaType#Sphere_ParticlesType#.csv"
            );

            _areaWithParticlesPath = Path.Combine(
                folder, 
                "Series#1_OuterRadius#100_AreaType#Sphere_ParticlesType#Icosahedron.csv"
            );
        }

        [Test]
        public void Write_AreaWithParticles_ReturnFilePath()
        {
            // Arrange
            var area = new SphereArea(1, 100, 5);

            Particle[] particles = [
                new IcosahedronParticle(5, new (0, 0, 0), new (360, 360, 360)),
                new IcosahedronParticle(3, new (10, 10, 10), new (360, 360, 360)),
                new IcosahedronParticle(1, new (-10, -10, -10), new (360, 360, 360)),
            ];

            area.Fill(particles, 3);

            // Act
            var file = _fileService.Write(area, 0);

            // Assert
            Assert.Pass(file);
        }

        [Test]
        public void Write_AreaWithoutParticles_ReturnFilePath()
        {
            // Arrange
            var area = new SphereArea(0, 100, 5);

            // Act
            var file = _fileService.Write(area, 0);

            // Assert
            Assert.Pass(file);
        }

        [Test]
        public void Read_AreaWithParticles_ReturnSphereAreaClass()
        {
            // Arrange
            if (_areaWithParticlesPath == null) 
                Assert.Fail();

            // Act
            var area = _fileService.Read(_areaWithParticlesPath);

            // Assert
            Assert.That(area.Particles.Any());
        }

        [Test]
        public void Read_AreaWithoutParticles_ReturnSphereAreaClass()
        {
            // Arrange
            if (_areaWithoutParticlesPath == null)
                Assert.Fail();

            // Act
            var area = _fileService.Read(_areaWithoutParticlesPath);

            // Assert
            Assert.That(area.Particles == null);
        }

        [Test]
        public void Read_RealAreaWithoutParticleCollision_True()
        {
            // Arrange
            var file = "Series#0_OuterRadius#1000_AreaType#Sphere_ParticlesType#Icosahedron.csv";

            var folder = Path.Combine(
                AppContext.BaseDirectory,
                "FilesForTests"
            );

            var path = Path.Combine(folder, file);

            if (!Path.Exists(path))
                throw new ArgumentException("Test file does not exist!");

            var area = _fileService.Read(path);

            var countOfCollision = 0;
            var particles = area.Particles.ToArray();
            int count = particles.Length;

            // Act
            Parallel.For(0, count, i =>
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (particles[i].Intersect(particles[j]))
                        Interlocked.Increment(ref countOfCollision);
                }
            });

            // Assert
            Assert.That(countOfCollision, Is.EqualTo(0));
        }
    }
}
