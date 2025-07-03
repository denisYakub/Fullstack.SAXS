using System.Security.Claims;
using Fullstack.SAXS.Application.Contracts;
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
        [HttpPost("sys")]
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

        [HttpGet("gen")]
        public IActionResult GetGenerations()
        {
            var json = sp.GetAll();

            return new OkObjectResult(json);
        }

        [HttpGet("gen/my")]
        public IActionResult GetMyGenerations()
        {
            var userId =
                User
                .FindFirstValue(ClaimTypes.NameIdentifier)
                ??
                throw new UnauthorizedAccessException("Need to login first!");

            var json = sp.GetAll(userId);

            return new OkObjectResult(json);
        }

        [HttpGet("sys/{id}")]
        public IActionResult GetSys([FromRoute] Guid id)
        {
            var json = sp.Get(id);

            return new OkObjectResult(json);
        }

        [HttpPost("sys/{id}/graf/phis")]
        public async Task<IActionResult> CreateSysPhiGraf([FromRoute] Guid id, [FromQuery] int layersNum = 5)
        {
            var userId =
                User
                .FindFirstValue(ClaimTypes.NameIdentifier)
                ??
                throw new UnauthorizedAccessException("Need to login first!");

            var html = await sysService.CreatePhiGrafAsync(userId, id, layersNum);

            return new ContentResult() { Content = html, ContentType = "text/html" };
        }

        [HttpPost("sys/{id}/graf/intenceOpt")]
        public async Task<IActionResult> CreateSysIntensOptGraf([FromRoute] Guid id, [FromBody] CreateIntensOptRequest request)
        {
            if (!request.isLegit)
                throw new BadHttpRequestException("Request is not correct!");

            var html = await
                sysService
                .CreateIntensOptGrafAsync(
                    id,
                    request.QMin, request.QMax,
                    request.QNum,
                    request.StepType
                );

            return new ContentResult() { Content = html, ContentType = "text/html" };
        }
    }
}
