using Fullstack.SAXS.Domain.Entities.Octrees;
using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Entities.Areas
{
    public class SphereArea : Area
    {
        private readonly IOctree? _octree;
        private readonly float _radius;

        public override float OuterRadius => _radius;
        public override AreaTypes AreaType => AreaTypes.Sphere;
        protected override IOctree? Octree => _octree;

        public SphereArea(int series, float radius, float maxParticleSize) 
            : base(series)
        {
            _radius = radius; 
            _octree = new OctreeWithClashingBoxes(radius);
        }

        public SphereArea(int series, float radius, ICollection<Particle>? particles)
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
