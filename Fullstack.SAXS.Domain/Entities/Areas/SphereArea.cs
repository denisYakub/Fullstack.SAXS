using Fullstack.SAXS.Domain.Commands;
using Fullstack.SAXS.Server.Domain.Entities.Octrees;
using Fullstack.SAXS.Server.Domain.Entities.Particles;
using Fullstack.SAXS.Server.Domain.Entities.Regions;
using Fullstack.SAXS.Server.Domain.Enums;

namespace Fullstack.SAXS.Server.Domain.Entities.Areas
{
    public class SphereArea : Area
    {
        private readonly Octree? _octree;
        private readonly float _radius;

        public override float OuterRadius => _radius;
        public override AreaTypes AreaType => AreaTypes.Sphere;
        protected override Octree? Octree => _octree;

        public SphereArea(int series, float radius, float maxParticleSize) 
            : base(series)
        {
            var outerRegion = new Region(new(0, 0, 0), radius * 2);
            var maxDepth = outerRegion.MaxDepth(3 * maxParticleSize);

            _radius = radius; 
            _octree = new Octree(maxDepth, outerRegion);
        }

        public SphereArea(int series, float radius, IReadOnlyCollection<Particle>? particles)
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
