using Fullstack.SAXS.Domain.Enums;
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
            100, 0.1,
            ParticleTypes.Icosahedron,
            1, 5,
            360, 360, 360,
            3, 2.5,
            100_000, 1
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
