using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Entities.Areas
{
    public abstract class Area(int series, ICollection<Particle> particles)
    {
        public int Series { get; init; } = series;

        private const int _retryToFillMax = 1_000;

        public IEnumerable<Particle> Particles 
        { 
            get
            {
                if (particles.Count > 0)
                    return particles;

                return Octree.GetAll();
            }
        }
        public ParticleTypes ParticlesType 
        {
            get
            {
                if (particles.Count > 0)
                    return particles.First().ParticleType;

                return Octree.GetAll().First().ParticleType;
            }
        }

        public abstract double OuterRadius { get; }
        public abstract AreaTypes AreaType { get; }
        protected abstract IOctree Octree { get; }

        public abstract bool Contains(Particle particle);

        public void Fill(IEnumerable<Particle> infParticles, int particleNumber)
        {
            if (particles.Count > 0)
                return;

            if (infParticles == null)
                throw new ArgumentNullException(nameof(infParticles), "Should be init.");

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
