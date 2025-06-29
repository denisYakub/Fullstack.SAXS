using Fullstack.SAXS.Domain.Entities.Octrees;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Entities.Areas
{
    public class SphereArea : Area
    {
        private readonly IOctree? _octree;
        private readonly double _radius;

        public override double OuterRadius => _radius;
        public override AreaTypes AreaType => AreaTypes.Sphere;
        protected override IOctree? Octree => _octree;

        public SphereArea(int series, double radius, double maxParticleSize) 
            : base(series)
        {
            _radius = radius; 
            _octree = new OctreeWithContainingBoxes(radius);
        }

        public SphereArea(int series, double radius, ICollection<Particle>? particles)
            : base(series, particles)
        {
            _radius = radius;
        }

        public override bool Contains(Particle particle)
        {
            var rDiff = _radius - particle.OuterSphereRadius;

            return particle.Center.LengthSquared() <= rDiff * rDiff;
        }
    }
}
