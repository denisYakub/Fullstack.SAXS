using System.Numerics;
using Fullstack.SAXS.Server.Application.Interfaces;
using Fullstack.SAXS.Server.Domain.Entities.Particles;
using Fullstack.SAXS.Server.Domain.Enums;
using Microsoft.AspNetCore.Components.Routing;

namespace Fullstack.SAXS.Server.Domain.Entities.Areas
{
    public class SphereArea(int series, float radius, IOctree<Particle> octree) : Area(series, octree)
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
