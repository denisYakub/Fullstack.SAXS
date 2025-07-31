using Fullstack.SAXS.Domain.Entities.Octrees;

namespace Fullstack.SAXS.Domain.Tests;

public class BoundTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Edge_RoundedDoubleEdge_Pass()
    {
        // Arrange
        var bound = new Bound(9.1305, new(0, 0, 0));

        // Act
        var edge = bound.Edge;

        // Assert
        Assert.That(edge, Is.EqualTo(16));
    }

    [Test]
    public void OctoSplit_CheckEdgesAndCenters_Pass()
    {
        // Arrange
        var bound = new Bound(9.1305, new(0, 0, 0));

        // Act
        var subBounds = Bound.OctoSplit(bound.Edge, bound.Center, bound.Depth);

        // Assert
        foreach (var subBound in subBounds)
        {
            Assert.Multiple(() =>
            {
                Assert.That(subBound.Edge, Is.EqualTo(8));
                Assert.That(subBound.Depth, Is.EqualTo(1));
            });

            Console.WriteLine(subBound.Center.ToString());
        }
    }
}
