using System.Drawing;
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
                1.19701672f,
                new(21.6355362f, 7.062855f, 6.27528763f),
                new(-137.3036f, 21.9578857f, 40.6453857f)
            );

            var particle2 = new IcosahedronParticle(
                1.26716781f,
                new(22.3404388f, 11.0143967f, 7.742401f),
                new(105.304993f , -259.407867f, -335.019257f)
            );

            // Act
            var result = particle2.Intersect(particle1);

            // Assert
            Assert.That(result);
        }
    }
}
