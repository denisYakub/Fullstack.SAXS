using System.Text;
using Castle.Components.DictionaryAdapter.Xml;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Application.Services;
using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Extensions;
using MathNet.Numerics.Distributions;
using Moq;

namespace Fullstack.SAXS.Application.Tests
{
    public class SysServiceTests
    {
        private SysService _service;

        [SetUp]
        public void Setup()
        {
            var factoryMock = new Mock<AreaFactory>();
            var storageMock = new Mock<IStorage>();
            var graphMock = new Mock<IGraphService>();
            var prtclFResolverMock = new Mock<IParticleFactoryResolver>();

            storageMock
                .Setup(s => s.GetAreaAsync(It.IsAny<Guid>()))
                .Returns<Guid>(
                    (id) => Task.FromResult(CreateSomeArea(100000)
                ));

            graphMock
                .Setup(g => g.GetHtmlPageAsync(It.IsAny<double[]>(), It.IsAny<double[]>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns<float[], float[]>((x, y) =>
                {
                    var builder = new StringBuilder();

                    for (int i = 0; i < x.Length; i++)
                    {
                        builder.AppendLine($"{x[i]} - {y[i]}");
                    }

                    return Task.FromResult(builder.ToString());
                });

            _service = new SysService(
                factoryMock.Object,
                prtclFResolverMock.Object,
                storageMock.Object, 
                graphMock.Object
            );
        }

        [Test]
        public async Task CreatePhiGrafAsync_5Layer_PhiValues()
        {
            // Arrange
            var layersNum = 5;

            // Act
            var phis = await _service.CreatePhiGraphAsync(Guid.Empty, Guid.NewGuid(), layersNum);

            // Assert
            Assert.Pass(phis);
        }

        [Test]
        public async Task CreateQs_BasicQs_TxtFileWithQs()
        {
            // Arrange
            var minQ = 0.02;
            var maxQ = 5.0;
            var numQ = 150;

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string pathQs = Path.Combine(documentsPath, "QI.txt");

            // Act
            var qs = SysService.CreateQs(minQ, maxQ, numQ);

            using (var writer = new StreamWriter(pathQs))
            {
                foreach (var q in qs)
                    writer.WriteLine(q);
            }

            // Assert
            Assert.Pass(string.Concat(qs));
        }

        private Area CreateSomeArea(int particleNum)
        {
            var areaRadius = 100;
            var maxParticleSize = 5;

            var area = new SphereArea(0, areaRadius, maxParticleSize);

            area.Fill(InfParticleGenerator(maxParticleSize, areaRadius), particleNum);

            return area;
        }

        private IEnumerable<Particle> InfParticleGenerator(float maxParticleSize, float areaRadius)
        {
            var random = new Random();
            var gamma = new Gamma(3, 2.5f);

            while (true)
            {
                var size = gamma.GetGammaRandom(1, maxParticleSize);

                var a = random.GetEvenlyRandom(-360, 360);
                var b = random.GetEvenlyRandom(-360, 360);
                var g = random.GetEvenlyRandom(-360, 360);

                var x = random.GetEvenlyRandom(-areaRadius, areaRadius);
                var y = random.GetEvenlyRandom(-areaRadius, areaRadius);
                var z = random.GetEvenlyRandom(-areaRadius, areaRadius);

                yield return new IcosahedronParticle(size, new(x, y, z), new(a, b, g));
            }
        }
    }
}
