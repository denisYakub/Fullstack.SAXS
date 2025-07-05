using System.Security.Claims;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Models;
using Fullstack.SAXS.Server.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.SAXS.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaxsController(ISysService sysService, ISpService sp) : ControllerBase
    {
        private Guid UserId
        {
            get
            {
                var userId =
                    User
                    .FindFirstValue(ClaimTypes.NameIdentifier)
                    ??
                    throw new UnauthorizedAccessException("Need to login first!");

                return Guid.Parse(userId);
            }
        }

        [HttpPost("systems")]
        public async Task<IActionResult> CreateSys([FromBody] CreateSysRequest request)
        {
            var dto = new CreateSysData(
                request.AreaSize, request.AreaNumber, request.ParticleNumber,
                request.ParticleType,
                request.ParticleMinSize, request.ParticleMaxSize,
                request.ParticleSizeShape, request.ParticleSizeScale,
                request.ParticleAlphaRotation,
                request.ParticleBetaRotation,
                request.ParticleGammaRotation
            );

            await sysService.CreateAsync(UserId, dto);

            return new OkResult();
        }

        [HttpGet("systems/{id}")]
        public async Task<IActionResult> GetSys([FromRoute] Guid id)
        {
            var json = await sp.GetAsync(id);

            return new OkObjectResult(json);
        }

        [HttpPost("systems/{id}/graphs/phi")]
        public async Task<IActionResult> CreateSysPhiGraph([FromRoute] Guid id, [FromQuery] int layersNum = 5)
        {
            var html = await sysService.CreatePhiGraphAsync(UserId, id, layersNum);

            return new ContentResult() { Content = html, ContentType = "text/html" };
        }

        [HttpPost("systems/{id}/graphs/intensity-opt")]
        public async Task<IActionResult> CreateSysIntensOptGraph([FromRoute] Guid id, [FromBody] CreateIntensOptRequest request)
        {
            var dto = new CreateQIData(
                request.QMin, request.QMax,
                request.QNum,
                request.StepType
            );
            var html = await sysService.CreateIntensOptGraphAsync(id, dto);

            return new ContentResult() { Content = html, ContentType = "text/html" };
        }

        [HttpGet("generations")]
        public async Task<IActionResult> GetGenerations()
        {
            var json = await sp.GetAllAsync();

            return new OkObjectResult(json);
        }

        [HttpGet("users/me/generations")]
        public async Task<IActionResult> GetMyGenerations()
        {
            var json = await sp.GetAllAsync(UserId);

            return new OkObjectResult(json);
        }
    }
}
