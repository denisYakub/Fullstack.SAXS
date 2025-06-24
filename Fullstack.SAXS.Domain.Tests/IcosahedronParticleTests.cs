using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Tests
{
    public class IcosahedronParticleTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Intersect_P1InsideP2_True()
        {
            // Arrenge
            var particle1 = new IcosahedronParticle(1, new(2, 2, 2), new(0, 0, 0));
            var particle2 = new IcosahedronParticle(3, new(0, 0, 0), new(0, 0, 0));

            // Act
            var result = particle1.Intersect(particle2);

            // Assert
            Assert.That(result);
        }

        [Test]
        public void Intersect_P2InsideP1_True()
        {
            // Arrenge
            var particle1 = new IcosahedronParticle(3, new(0, 0, 0), new(0, 0, 0));
            var particle2 = new IcosahedronParticle(1, new(2, 2, 2), new(0, 0, 0));

            // Act
            var result = particle1.Intersect(particle2);

            // Assert
            Assert.That(result);
        }

        [Test]
        public void Intersect_P2VertexInsideP1_True()
        {
            // Arrenge
            var particle1 = new IcosahedronParticle(3, new(0, 0, 0), new(0, 0, 0));
            var particle2 = new IcosahedronParticle(1, new(4f, 4f, 2), new(0, 0, 0));

            // Act
            var result = particle1.Intersect(particle2);

            // Assert
            Assert.That(result);
        }

        [Test]
        public void Intersect_P1DontIntersectWithP2_False()
        {
            // Arrenge
            var particle1 = new IcosahedronParticle(3, new(0, 0, 0), new(0, 0, 0));
            var particle2 = new IcosahedronParticle(1, new(4.5f, 4.5f, 2), new(0, 0, 0));

            // Act
            var result = particle1.Intersect(particle2);

            // Assert
            Assert.That(!result);
        }

        [Test]
        public void Intersect_RealCollision_True()
        {
            // Arrenge
            var particle1 = new IcosahedronParticle(
                3.0210254f,
                new(-2.4270363f, 6.009531f, 12.535042f),
                new(180.69635f, 352.37677f, 186.3266f)
            );

            var particle2 = new IcosahedronParticle(
                1.8071625f,
                new(-3.560812f, 3.9010715f, 13.0229225f),
                new(-149.30103f, -117.1031f, 87.887665f)
            );

            // Act
            var result = particle2.Intersect(particle1);

            // Assert
            Assert.That(result);
        }
    }
}
