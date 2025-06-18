using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Server.Domain.Entities.Areas;
using Fullstack.SAXS.Server.Domain.Entities.Octrees;
using Fullstack.SAXS.Server.Domain.Entities.Regions;
using Fullstack.SAXS.Server.Infastructure.Factories;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.SAXS.Server.Infastructure.Services
{
    public class SysService(AreaParticleFactory factory, IStorage storage)
    {
        public void Create(
            string? userId,
            float AreaSize, int AreaNumber, int ParticleNumber,
            float ParticleMinSize, float ParticleMaxSize,
            float ParticleSizeShape, float ParticleSizeScale,
            float ParticleAlphaRotation,
            float ParticleBetaRotation,
            float ParticleGammaRotation
        )
        {
            try
            {
                var idUser = Guid.Parse(userId);

                var startRegion = new Region(new(0, 0, 0), AreaSize * 2);
                var octree = new Octree(startRegion.MaxDepth(3 * ParticleMaxSize), startRegion);

                var areas = factory.GetAreas(AreaSize, AreaNumber, ParticleMaxSize);

                Parallel.ForEach(areas, area => {
                    var infParticles =
                        factory
                        .GetInfParticles(
                            ParticleMinSize, ParticleMaxSize,
                            ParticleSizeShape, ParticleSizeScale,
                            ParticleAlphaRotation,
                            ParticleBetaRotation,
                            ParticleGammaRotation,
                            -AreaSize, AreaSize,
                            -AreaSize, AreaSize,
                            -AreaSize, AreaSize
                        );

                    area.Fill(infParticles, ParticleNumber);

                    storage.AddAsync(area);
                });

                storage.SaveAsync(idUser);
            }
            catch (ArgumentNullException)
            {
                throw new UnauthorizedAccessException("userId is null");
            }
            catch (FormatException)
            {
                throw new UnauthorizedAccessException("userId is not Guid");
            }
        }

        public IHtmlContent CreateIntensOptGraf(
            Guid id,
            float QMin, float QMax, int QNum
        )
        {
            throw new NotImplementedException();
        }

        public IHtmlContent CreatePhiGraf(Guid id, int layersNum)
        {
            throw new NotImplementedException();
        }

        public JsonResult Get(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
