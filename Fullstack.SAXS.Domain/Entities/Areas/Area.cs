using System.Data;
using System.Numerics;
using Fullstack.SAXS.Server.Domain.Entities.Octrees;
using Fullstack.SAXS.Server.Domain.Entities.Particles;
using Fullstack.SAXS.Server.Domain.Entities.Regions;
using Fullstack.SAXS.Server.Domain.Enums;

namespace Fullstack.SAXS.Server.Domain.Entities.Areas
{
    public abstract class Area
    {
        public static readonly Vector3 Center = new(0, 0, 0);

        private static readonly int _retryToFillMax = 1000;

        public readonly int Series;

        private Octree _octree;

        public IEnumerable<Particle> Particles 
        { 
            get
            {
                return _octree.GetAll();
            }
        }
        public ParticleTypes ParticlesType 
        {
            get
            {
                return 
                    _octree
                    .GetAll()
                    .First()
                    .ParticleType;
            }
        }

        public abstract float OuterRadius { get; }
        public abstract AreaTypes AreaType { get; }

        protected Area(int series, float maxParticleSize)
        {
            Series = series;

            var outerRegion = new Region(new (0, 0, 0), OuterRadius * 2);
            var maxDepth = outerRegion.MaxDepth(3 * maxParticleSize);

            _octree = new Octree(maxDepth, outerRegion);
        }

        public abstract bool Contains(Particle particle);

        public void Fill(IEnumerable<Particle> infParticles, int particleNumber)
        {
            var retryCount = 0;
            var particleCount = 0;

            using var particleGenerator = infParticles.GetEnumerator();
            while (particleGenerator.MoveNext())
            {
                if (retryCount == _retryToFillMax || particleCount == particleNumber)
                    break;

                var particle = particleGenerator.Current;

                if (Contains(particle) && _octree.Add(particle))
                {
                    particleCount++;
                    retryCount = 0;
                }
                else
                {
                    retryCount++;
                }
            }
        }
    }
}
