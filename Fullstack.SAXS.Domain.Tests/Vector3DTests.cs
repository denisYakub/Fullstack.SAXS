using System.Numerics;
using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Domain.Tests;

public class Vector3DTests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void Length_CompareTimeToLengthQ_Slower()
    {
        // Arrange
        var vector = new Vector3D(5, 5, 5);
        double length;

        // Act
        for (int i = 0; i < 1_000_000; i++)
            length = vector.Length();

        // Assert
        Assert.Pass();
    }

    [Test]
    public void LengthQ_CompareTimeToLength_Faster()
    {
        // Arrange
        var vector = new Vector3D(5, 5, 5);
        double length;

        // Act
        for (int i = 0; i < 1_000_000; i++)
            length = vector.LengthQ();

        // Assert
        Assert.Pass();
    }

    [Test]
    public void LengthQ_CompareToRegulareLength_Same()
    {
        // Arrange
        var vector = new Vector3D(5.5, 5.5, 5.5);

        // Act
        var reg = vector.Length();
        var inv = vector.LengthQ();

        // Assert
        Console.WriteLine(inv);
        Console.WriteLine(reg);
        Assert.That(Math.Abs(inv - reg) < 1e-4);
    }

    [Test]
    public void Length_Vector3MethodEqualsToVector3D_True()
    {
        // Arrange
        var vectorD = new Vector3D(5.5, 5.5, 5.5);
        var vector = new Vector3(5.5f, 5.5f, 5.5f);

        // Act
        var d = vectorD.Length();
        var reg = vector.Length();

        // Assert
        Assert.That(Math.Abs((double)d - reg) < 1e-6);
    }

    [Test]
    public void Parse_ToStrAndBack_Same()
    {
        // Arrange
        var vector = new Vector3D(5.521, 5.521, 5.521);

        // Act
        var str = vector.ToString();
        var back = Vector3D.Parse(str);

        // Assert
        Assert.That(vector.Equals(back));
    }
}
