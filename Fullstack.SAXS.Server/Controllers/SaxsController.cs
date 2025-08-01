using System.Security.Claims;
using Fullstack.SAXS.Application.Commands;
using Fullstack.SAXS.Application.Queries;
using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Server.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.SAXS.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaxsController(IMediator mediator) : ControllerBase
    {
        private Guid UserId
        {
            get
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? throw new UnauthorizedAccessException("User is not authenticated.");

                if (!Guid.TryParse(userIdStr, out var userId))
                    throw new FormatException("Invalid user ID format in claims.");

                return userId;
            }
        }

        [HttpPost("systems")]
        public async Task<IActionResult> CreateSys([FromBody] CreateSysRequest request)
        {
            var dto = new SystemCreateDto(
                UserId,
                request.AreaSize, 
                request.AreaNumber, request.ParticleNumber,
                AreaTypes.Sphere, request.ParticleType,
                request.ParticleMinSize, request.ParticleMaxSize,
                request.ParticleSizeShape, request.ParticleSizeScale,
                request.ParticleAlphaRotation,
                request.ParticleBetaRotation,
                request.ParticleGammaRotation
            );

            await mediator
                .Send(new CreateTaskToCreateSystemCommand(dto))
                .ConfigureAwait(false);  

            return new OkResult();
        }

        [HttpGet("systems/{id}")]
        public async Task<IActionResult> GetSys([FromRoute] Guid id)
        {
            var json = await mediator
                .Send(new GetSystemQuery(id))
                    .ConfigureAwait(false);

            return new OkObjectResult(json);
        }

        [HttpPost("systems/{id}/graphs/phi")]
        public async Task<IActionResult> CreateSysPhiGraph([FromRoute] Guid id, [FromQuery] int layersNum)
        {
            var dto = new DensityCreateDTO(
                UserId, id, layersNum
            );

            var html = await mediator
                .Send(new CreateDensityGraphCommand(dto))
                .ConfigureAwait(false);

            return new ContentResult() { Content = html, ContentType = "text/html" };
        }

        [HttpPost("systems/{id}/graphs/intensity-opt")]
        public async Task<IActionResult> CreateSysIntensOptGraph([FromRoute] Guid id, [FromBody] CreateIntensOptRequest request)
        {
            var model = new IntensityCreateDTO(
                id,
                request.QMin, request.QMax,
                request.QNum,
                request.StepType
            );

            var html = await mediator
                .Send(new CreateIntenseOptGraphCommand(model))
                .ConfigureAwait(false);

            return new ContentResult() { Content = html, ContentType = "text/html" };
        }

        [HttpGet("generations")]
        public async Task<IActionResult> GetGenerations([FromQuery] bool users)
        {
            var model = new GenerationGetFilterDTO(
                users ? UserId : null
            );

            var json = await mediator
                .Send(new GetAllGenerationsQuery(model))
                .ConfigureAwait(false);

            return new OkObjectResult(json);
        }
    }
}
