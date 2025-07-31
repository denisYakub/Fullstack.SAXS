namespace Fullstack.SAXS.Domain.Tests;

public class Vector3DTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        // Arrange
        var nanDouble = double.NaN;

        // Act
        var compareTwoNans = nanDouble == nanDouble;

        // Assert
        Console.WriteLine(nanDouble);
        Assert.Pass($"NaN-{nanDouble}");
    }
}
