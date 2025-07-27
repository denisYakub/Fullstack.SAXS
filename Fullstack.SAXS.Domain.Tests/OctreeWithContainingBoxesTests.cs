using Fullstack.SAXS.Domain.Entities.Octrees;

namespace Fullstack.SAXS.Domain.Tests;

public class OctreeWithContainingBoxesTests
{
    private static double _outerAreaR = 100.0;
    private OctreeWithContainingBoxes _octree;

    [SetUp]
    public void Setup()
    {
        _octree = new OctreeWithContainingBoxes(_outerAreaR);
    }

    [Test]
    public void GetAll_NoIntersection_Pass()
    {
        // Arrange
        OctreeWithBoundingBoxIndexesTests.FillOctree(_octree, _outerAreaR, 500_000);

        // Act
        var insertedParticles = _octree.GetAll().ToArray();

        // Assert
        bool hasIntersection = false;
        object lockObj = new();

        Parallel.For(0, insertedParticles.Length, (i, state) =>
        {
            for (int j = i + 1; j < insertedParticles.Length; j++)
            {
                if (insertedParticles[i].Intersect(insertedParticles[j]))
                {
                    lock (lockObj)
                        hasIntersection = true;

                    state.Stop();
                    return;
                }
            }
        });

        if (hasIntersection)
            Assert.Fail("Intersection detected");

        Assert.Pass("No intersection");
    }
}
