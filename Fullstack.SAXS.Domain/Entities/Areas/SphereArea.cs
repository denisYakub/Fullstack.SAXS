using System.Numerics;
using Fullstack.SAXS.Server.Domain.Entities.Octrees;
using Fullstack.SAXS.Server.Domain.Entities.Particles;
using Fullstack.SAXS.Server.Domain.Enums;
using MathNet.Numerics;

namespace Fullstack.SAXS.Server.Domain.Entities.Areas
{
    public class SphereArea : Area
    {
        public override float OuterRadius => _radius;

        public override AreaTypes AreaType => AreaTypes.Sphere;

        private readonly float _radius;

        public SphereArea(int series, float radius, float maxParticleSize) 
            : base(series, maxParticleSize)
        {
            _radius = radius;
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
