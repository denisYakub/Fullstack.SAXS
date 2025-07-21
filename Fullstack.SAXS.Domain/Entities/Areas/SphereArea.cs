using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Octrees;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Entities.Areas
{
    public class SphereArea(int series, double radius, ICollection<Particle> particles) : Area(series, particles)
    {
        private readonly IOctree _octree = new OctreeWithList();

        public override double OuterRadius => radius;
        public override AreaTypes AreaType => AreaTypes.Sphere;
        protected override IOctree Octree => _octree;

        public SphereArea(int series, double radius)
            : this(series, radius, new List<Particle>()) 
        {
            _octree = new OctreeWithContainingBoxes(radius);
        }

        public override bool Contains(Particle particle)
        {
            if (particle == null)
                throw new ArgumentNullException(nameof(particle), "Shouldn't be null");

            var rDiff = radius - particle.OuterSphereRadius;

            return particle.Center.LengthSquared() <= rDiff * rDiff;
        }
    }
}
