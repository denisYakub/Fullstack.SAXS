using Fullstack.SAXS.Server.Domain.Entities.Octrees;
using Fullstack.SAXS.Server.Domain.Entities.Regions;
using Fullstack.SAXS.Server.Infastructure.Factories;

namespace Fullstack.SAXS.Tests
{
    public class Tests
    {
        private int areaR = 3000, areaNumber = 3, particleNumber = 100_000;
        private SphereIcosahedronFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new SphereIcosahedronFactory();
        }

        [Test]
        public void Test_SphereIcosahedronFactory_Methods()
        {
            //Init
            var startRegion = new Region(new(0, 0, 0), areaR * 2);
            var octree = new Octree(startRegion.MaxDepth(3 * 5), startRegion);
            var areas = 
                _factory
                .GetAreas(areaR, areaNumber, 5);
            //Action
            Parallel.ForEach(areas, area =>
            {
                var infParticles =
                _factory
                .GetInfParticles(
                    1, 5,
                    3, 2.5f,
                    360, 360, 360,
                    -areaR, areaR,
                    -areaR, areaR,
                    -areaR, areaR
                );

                area.Fill(infParticles, particleNumber);

                var particles = area.Particles.ToList();
            });
            //Assert
            Assert.Pass();
        }
    }
}
