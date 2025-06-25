using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Domain.Tests;

public class EulerAnglesTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Parse_FromTo_Equal()
    {
        // Arrange
        var angles = new EulerAngles(5, 10, 15);
        var anglesStr = angles.ToString();

        // Act
        var anglesBack = EulerAngles.Parse(anglesStr);

        // Assert
        Assert.That(anglesBack.PraecessioAngle, Is.EqualTo(angles.PraecessioAngle));
        Assert.That(anglesBack.NutatioAngle, Is.EqualTo(angles.NutatioAngle));
        Assert.That(anglesBack.ProperRotationAngle, Is.EqualTo(angles.ProperRotationAngle));
    }
}
