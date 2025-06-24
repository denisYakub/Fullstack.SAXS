using System.Security.Claims;
using Fullstack.SAXS.Application;
using Fullstack.SAXS.Server.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.SAXS.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaxsController(SysService sysService) : ControllerBase
    {
        [HttpPost("sys/create")]
        public IActionResult CreateSys([FromBody] CreateSysRequest request)
        {
            var userId = 
                User
                .FindFirstValue(ClaimTypes.NameIdentifier)
                ??
                throw new UnauthorizedAccessException("Need to login first!");

            if (!request.IsLegit)
                throw new BadHttpRequestException("Request is not correct!");

            sysService
                .Create(
                    userId,
                    request.AreaSize, request.AreaNumber, request.ParticleNumber,
                    request.ParticleMinSize, request.ParticleMaxSize,
                    request.ParticleSizeShape, request.ParticleSizeScale,
                    request.ParticleAlphaRotation,
                    request.ParticleBetaRotation,
                    request.ParticleGammaRotation
                );

            return new OkResult();
        }

        [HttpGet("sys/get/{id}")]
        public IActionResult GetSys([FromRoute] Guid id)
        {
            var json = sysService.Get(id);

            return new OkObjectResult(json);
        }

        [HttpPost("sys/create/phi/graf")]
        public async Task<IActionResult> CreateSysPhiGraf([FromQuery] Guid id, [FromQuery] int layersNum)
        {
            var html = await sysService.CreatePhiGrafAsync(id, layersNum);

            return new ContentResult() { Content = html, ContentType = "text/html" };
        }

        [HttpPost("sys/create/intens/opt/graf")]
        public async Task<IActionResult> CreateSysIntensOptGraf([FromQuery] Guid id, [FromBody] CreateIntensOptRequest request)
        {
            if (!request.isLegit)
                throw new BadHttpRequestException("Request is not correct!");

            var html = await
                sysService
                .CreateIntensOptGrafAsync(
                    id,
                    request.QMin, request.QMax,
                    request.QNum
                );

            return new ContentResult() { Content = html, ContentType = "text/html" };
        }
    }
}
