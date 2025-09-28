using Fullstack.SAXS.Domain.Abstractions.Factories;
using Fullstack.SAXS.Domain.Abstractions.Resolvers;
using Fullstack.SAXS.Domain.Abstractions.Services;
using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Entities.Areas;

namespace Fullstack.SAXS.Services
{
    public class SystemService(AreaFactory areaF, IParticleFactoryResolver prtclFResolver) : ISysService
    {
        public IReadOnlyCollection<Area> Create(AreaCreateDTO areDTO, ParticleCreateDTO particleDTO)
        {
            if (areDTO == null)
                throw new ArgumentNullException(nameof(areDTO), "Shouldn't be null.");

            if (particleDTO == null)
                throw new ArgumentNullException(nameof(particleDTO), "Shouldn't be null.");

            var areas = areaF.GetAreas(areDTO).ToArray();

            Parallel.For(0, areas.Length, i => {
                var infParticles =
                    prtclFResolver
                    .Resolve(particleDTO.Type)
                    .GetParticlesInf(particleDTO);

                areas[i].Fill(infParticles, particleDTO.Number);
            });

            return areas;
        }
    }
}
