using Fullstack.SAXS.Domain.Entities.Octrees;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Entities.Areas
{
    public abstract class Area
    {
        public readonly int Series;

        private static readonly int _retryToFillMax = 1_000;

        private IReadOnlyCollection<Particle>? _particles;

        public IEnumerable<Particle>? Particles 
        { 
            get
            {
                if (_particles != null)
                    return _particles;

                if (Octree != null)
                    return Octree.GetAll();

                return null;
            }
        }
        public ParticleTypes? ParticlesType 
        {
            get
            {
                if (_particles != null)
                    return _particles.First().ParticleType;

                if (Octree != null)
                    return Octree.GetAll().First().ParticleType;

                return null;
            }
        }

        public abstract float OuterRadius { get; }
        public abstract AreaTypes AreaType { get; }
        protected abstract Octree? Octree { get; }

        protected Area(int series)
        {
            Series = series;
        }

        protected Area(int series, IReadOnlyCollection<Particle>? particles)
        {
            Series = series;
            _particles = particles;
        }

        public abstract bool Contains(Particle particle);

        public void Fill(IEnumerable<Particle> infParticles, int particleNumber)
        {
            if (_particles != null && Octree == null)
                return;

            var retryCount = 0;
            var particleCount = 0;

            using var particleGenerator = infParticles.GetEnumerator();
            while (particleGenerator.MoveNext())
            {
                if (retryCount == _retryToFillMax || particleCount == particleNumber)
                    break;

                var particle = particleGenerator.Current;

                if (Contains(particle) && Octree.Add(particle))
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
