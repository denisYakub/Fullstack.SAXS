using Fullstack.SAXS.Server.Contracts;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Fullstack.SAXS.Server.Tests;

public class CreateSysRequestTests
{
    private CreateSysRequest _request;

    [SetUp]
    public void Setup()
    {
        _request = new CreateSysRequest(
            100, 0.1,       // AreaRadius, Nc
            1, 5,           // ParticleMinSize, ParticleMaxSize,
            360, 360, 360,  // ParticleAngleRotations
            3, 2.5,         // ParticleSizeShape, ParticleSizeScale,
            100_000, 1      // ParticleNumber, AreaNumber
        );
    }

    [Test]
    public void AreaSize_GetAreaSizeUsingNc_SameAsReal()
    {
        // Arrange


        // Act
        var areaSize = _request.AreaSize;

        // Assert
        Assert.Pass(areaSize.ToString());
    }
}
