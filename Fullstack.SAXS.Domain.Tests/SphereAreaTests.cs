using Fullstack.SAXS.Domain.Commands;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;
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
        public void Fill_WithSmallAnount_AreaWith100Particles()
        {
            // Arrange
            var area = new SphereArea(0, _areaRadius, _maxParticleSize);

            // Act
            area.Fill(_infParticles, 100);

            // Assert
            Console.WriteLine(area.Particles.Count());

            Assert.That(area.Particles.Any());
        }

        [Test]
        public void Fill_WithBigAnount_AreaWith500000Particles()
        {
            // Arrange
            var area = new SphereArea(0, _areaRadius, _maxParticleSize);

            // Act
            area.Fill(_infParticles, 500_000);

            // Assert
            Console.WriteLine(area.Particles.Count());

            Assert.That(area.Particles.Any());
        }

        [Test]
        public void Fill_WithSmallAnountAndCheckForCollision_AreaWith1000ParticlesAndFalse()
        {
            // Arrange
            var area = new SphereArea(0, _areaRadius, _maxParticleSize);

            var countOfCollision = 0;

            // Act
            area.Fill(_infParticles, 100000);

            var particles = area.Particles.ToArray();

            for (int i = 0; i < particles.Count(); i++)
            {
                for (int j = i + 1; j < particles.Count(); j++)
                {
                    if (particles.ElementAt(i).Intersect(particles.ElementAt(j)))
                        countOfCollision++;
                }
            }

            // Assert
            Console.WriteLine(area.Particles.Count());

            Assert.That(area.Particles.Any());
            Assert.That(countOfCollision, Is.EqualTo(0));
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
