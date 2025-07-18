﻿using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Infrastructure.Factories
{
    public class SphereFactory : AreaFactory
    {
        public override IEnumerable<Area> GetAreas(double areaSize, int number, double maxParticleSize)
        {
            for (int i = 0; i < number; i++) 
                yield return new SphereArea(i, areaSize, maxParticleSize);
        }
    }
}
