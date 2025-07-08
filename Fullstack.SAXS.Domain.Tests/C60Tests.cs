using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Tests;

public class C60Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Size_OuterBiggerThenInnerRadius_True()
    {
        // Arrange
        var c60 = new C60(5, new(0, 0, 0), new(0, 0, 0));

        // Act
        var innerR = c60.InnerSphereRadius;
        var outerR = c60.OuterSphereRadius;

        //Assert
        Assert.That(innerR, Is.LessThan(outerR));
    }
}
