using Fullstack.SAXS.Server.API.Dtos;
using Fullstack.SAXS.Server.Application.Interfaces;
using Fullstack.SAXS.Server.Domain.Entities.Areas;
using Fullstack.SAXS.Server.Domain.Entities.Octrees;
using Fullstack.SAXS.Server.Domain.Entities.Regions;
using Fullstack.SAXS.Server.Infastructure.Factories;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.SAXS.Server.Infastructure.Services
{
    public class SysService(AreaParticleFactory factory, IRepository<Area> repository) : ISysService
    {
        public void Create(string? userId, CreateSysRequest request)
        {
            try
            {
                var idUser = Guid.Parse(userId);

                if (!request.IsLegit)
                    throw new BadHttpRequestException("Exception in CreateSysRequest");

                var startRegion = new Region(new(0, 0, 0), request.AreaSize * 2);
                var octree = new Octree(startRegion.MaxDepth(3 * request.ParticleMaxSize), startRegion);

                var areas = factory.GetAreas(request.AreaSize, request.AreaNumber, octree);

                Parallel.ForEach(areas, area => {
                    var infParticles =
                        factory
                        .GetInfParticles(
                            request.ParticleMinSize, request.ParticleMaxSize,
                            request.ParticleSizeShape, request.ParticleSizeScale,
                            request.ParticleAlphaRotation,
                            request.ParticleBetaRotation,
                            request.ParticleGammaRotation,
                            -request.AreaSize, request.AreaSize,
                            -request.AreaSize, request.AreaSize,
                            -request.AreaSize, request.AreaSize
                        );

                    area.Fill(infParticles, request.ParticleNumber);

                    repository.AddAsync(area);
                });

                repository.SaveAsync(idUser);
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

        public IHtmlContent CreateIntensOptGraf(Guid id, CreateIntensOptRequest request)
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
