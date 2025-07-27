using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Octrees;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Extensions;
using MathNet.Numerics.Distributions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Fullstack.SAXS.Domain.Tests
{
    public class OctreeWithBoundingBoxIndexesTests
    {
        private static double _outerAreaR = 100.0;
        private OctreeWithBoundingBoxIndexes _octree;

        [SetUp]
        public void Setup()
        {
            _octree = new OctreeWithBoundingBoxIndexes(_outerAreaR);
        }

        [Test]
        public void GetAll_NoIntersection_Pass()
        {
            // Arrange
            FillOctree(_octree, 1_000);

            // Act
            var insertedParticles = _octree.GetAll().ToArray();

            // Assert
            for (int i = 0; i < insertedParticles.Length;  i++)
                for (int j = i + 1; j < insertedParticles.Length - 1; j++)
                {
                    if (insertedParticles[i].Intersect(insertedParticles[j]))
                        Assert.Fail("Intersection detected");
                }

            Assert.Pass("No intersection");
        }

        private static void FillOctree(IOctree octree, int particleNum)
        {
            var count = 0;
            var attempt = 0;

            var random = new Random();
            var gamma = new Gamma(3, 2.5);

            while (count != particleNum && attempt != 1_000)
            {
                var size = gamma.GetGammaRandom(1, 5);

                var a = random.GetEvenlyRandom(-360, 360);
                var b = random.GetEvenlyRandom(-360, 360);
                var g = random.GetEvenlyRandom(-360, 360);

                var x = random.GetEvenlyRandom(-_outerAreaR, _outerAreaR);
                var y = random.GetEvenlyRandom(-_outerAreaR, _outerAreaR);
                var z = random.GetEvenlyRandom(-_outerAreaR, _outerAreaR);

                var particle = new IcosahedronParticle(size, new(x, y, z), new(a, b, g));

                if (octree.TryToAdd(particle))
                {
                    count++;
                    attempt = 0;
                }
                else
                {
                    attempt++;
                }
            }
        }
    }
}
