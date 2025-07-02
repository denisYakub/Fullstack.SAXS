using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Domain.Tests;

public class EulerAnglesTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Parse_ToStringAndBack_Equal()
    {
        // Arrange
        var angles = new EulerAngles(5, 10, 15);

        // Act
        var anglesStr = angles.ToString();
        var anglesBack = EulerAngles.Parse(anglesStr);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(anglesBack.PraecessioAngle, Is.EqualTo(angles.PraecessioAngle));
            Assert.That(anglesBack.NutatioAngle, Is.EqualTo(angles.NutatioAngle));
            Assert.That(anglesBack.ProperRotationAngle, Is.EqualTo(angles.ProperRotationAngle));
        });
    }
}
