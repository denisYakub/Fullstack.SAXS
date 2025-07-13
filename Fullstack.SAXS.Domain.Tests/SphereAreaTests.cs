using System.Globalization;
using System.IO;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Extensions;
using MathNet.Numerics.Distributions;

namespace Fullstack.SAXS.Domain.Tests
{
    public class ShereAreaTests
    {
        private IEnumerable<Particle> _infParticles;
        private float _maxParticleSize = 5;
        private float _areaRadius = 1000;

        [SetUp]
        public void Setup()
        {
            _infParticles = InfParticleGenerator();
        }

        [Test]
        public void Fill_With100KParticles_Pass()
        {
            // Arrange
            var area = new SphereArea(0, _areaRadius, _maxParticleSize);

            // Act
            area.Fill(_infParticles, 100_000);

            // Assert
            Assert.That(area.Particles.Any());
        }

        [Test]
        public void Fill_With500KParticles_Pass()
        {
            // Arrange
            var area = new SphereArea(0, _areaRadius, _maxParticleSize);

            // Act
            area.Fill(_infParticles, 500_000);

            // Assert
            Assert.That(area.Particles.Any());
        }

        [Test]
        public void Fill_With100KParticlesAndCheckCollision_NoCollision()
        {
            // Arrange
            var area = new SphereArea(0, _areaRadius, _maxParticleSize);

            var countOfCollision = 0;

            // Act
            area.Fill(_infParticles, 100_000);

            var particles = area.Particles.ToArray();
            var count = particles.Length;

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

        [Test]
        public void Fill_With10KParticlesAndSaveСoordinatesToDocuments_Pass()
        {
            // Arrange
            var area = new SphereArea(0, 200, _maxParticleSize);

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string pathParticlesCoord = Path.Combine(documentsPath, "1MBN_coordinates_of_atoms.txt");
            string pathParticlesNel = Path.Combine(documentsPath, "1MBN_Nel_in_atoms.txt");

            // Act
            area.Fill(_infParticles, 10_000);

            var particles = area.Particles.ToArray();

            using (var writer = new StreamWriter(pathParticlesCoord))
            {
                foreach (var particle in particles)
                {

                    var c = particle.Center;
                    string line = string.Format(CultureInfo.InvariantCulture, "{0:F6} {1:F6} {2:F6}", c.X, c.Y, c.Z);
                    writer.WriteLine(line);
                }
            }

            using (var writer = new StreamWriter(pathParticlesNel))
            {
                for (int i = 1; i <= particles.Length; i++)
                {
                    writer.WriteLine(i);
                }
            }

            // Assert
            Assert.Pass(pathParticlesCoord);
        }

        private IEnumerable<Particle> InfParticleGenerator()
        {
            var random = new Random();
            var gamma = new Gamma(3, 2.5f);

            while (true)
            {
                var size = gamma.GetGammaRandom(1, _maxParticleSize);

                var a = random.GetEvenlyRandom(-360, 360);
                var b = random.GetEvenlyRandom(-360, 360);
                var g = random.GetEvenlyRandom(-360, 360);

                var x = random.GetEvenlyRandom(-_areaRadius, _areaRadius);
                var y = random.GetEvenlyRandom(-_areaRadius, _areaRadius);
                var z = random.GetEvenlyRandom(-_areaRadius, _areaRadius);

                yield return new IcosahedronParticle(size, new(x, y, z), new(a, b, g));
            }
        }
    }
}
