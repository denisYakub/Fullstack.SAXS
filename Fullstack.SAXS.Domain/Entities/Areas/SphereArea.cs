using System.Numerics;
using Fullstack.SAXS.Server.Domain.Entities.Octrees;
using Fullstack.SAXS.Server.Domain.Entities.Particles;
using Fullstack.SAXS.Server.Domain.Enums;

namespace Fullstack.SAXS.Server.Domain.Entities.Areas
{
    public class SphereArea(int series, float radius, float maxParticleSize) : Area(series, maxParticleSize)
    {
        public override float OuterRadius => radius;

        public override AreaTypes AreaType => AreaTypes.Sphere;

        public override bool Contains(Particle particle)
        {
            var rDiff = radius - particle.OuterSphereRadius;

            return particle.Center.LengthSquared() <= rDiff * rDiff;
         }
    }
}
