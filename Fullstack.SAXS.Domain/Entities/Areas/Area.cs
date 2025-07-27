using Fullstack.SAXS.Domain.Common;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Entities.Areas
{
    public abstract class Area(int series, ICollection<Particle> particles)
    {
        private const int _retryToFillMax = 1_000;

        public int Series { get; init; } = series;

        public IEnumerable<Particle> Particles 
        { 
            get
            {
                if (particles.Count > 0)
                    return particles;
                
                if (Octree.GetAll().Any())
                    return Octree.GetAll();

                return [];
            }
        }
        public ParticleTypes ParticlesType 
        {
            get
            {
                if (particles.Count > 0)
                    return particles.First().ParticleType;

                if (Octree.GetAll().Any())
                    return Octree.GetAll().First().ParticleType;

                return default;
            }
        }

        public abstract double OuterRadius { get; }
        public abstract AreaTypes AreaType { get; }
        protected abstract IOctree Octree { get; }

        public void Fill(IEnumerable<Particle> incummingParticles, int maxParticleNumber)
        {
            if (particles.Count > 0)
                return;

            if (incummingParticles == null)
                throw new ArgumentNullException(nameof(incummingParticles), "Should be init.");

            var retryCount = 0;
            var particleCount = 0;

            using var particleGenerator = incummingParticles.GetEnumerator();
            while (particleGenerator.MoveNext())
            {
                if (retryCount == _retryToFillMax || particleCount == maxParticleNumber)
                    break;

                var particle = particleGenerator.Current;

                if (Contains(particle) && Octree.TryToAdd(particle))
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

        public abstract bool Contains(Particle particle);
    }
}
