using Fullstack.SAXS.Application.Services;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Application.Tests
{
    public class SysServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateQs_Linear_Pass()
        {
            // Arrange
            var qMin = 0.02;
            var qMax = 5.0;
            var qNum = 150;

            // Act
            var qVector = SysService.CreateQs(qMin, qMax, qNum);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(qVector, Has.Length.EqualTo(qNum));
                Assert.That(qVector.First(), Is.AtLeast(qMin));
                Assert.That(qVector.Last(), Is.AtMost(qMax));
            });
        }

        [Test]
        public void CreateQs_Logarithmic_Pass()
        {
            // Arrange
            var qMin = 0.02;
            var qMax = 5.0;
            var qNum = 150;

            // Act
            var qVector = SysService.CreateQs(qMin, qMax, qNum, StepTypes.Logarithmic);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(qVector, Has.Length.EqualTo(qNum));
                Assert.That(qVector.First(), Is.AtLeast(qMin));
                Assert.That(qVector.Last(), Is.AtMost(qMax));
            });
        }
    }
}
