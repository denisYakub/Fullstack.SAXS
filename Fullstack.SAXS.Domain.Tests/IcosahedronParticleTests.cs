using Fullstack.SAXS.Server.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Tests;

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
        var particle1 = new IcosahedronParticle(3, new (0, 0, 0), new (0, 0, 0));
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
}
