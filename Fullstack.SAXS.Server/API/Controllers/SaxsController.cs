using System.Security.Claims;
using Fullstack.SAXS.Server.API.Dtos;
using Fullstack.SAXS.Server.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.SAXS.Server.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaxsController(ISysService sysService) : ControllerBase
    {
        [HttpPost("sys/create")]
        public IActionResult CreateSys([FromBody] CreateSysRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            sysService.Create(userId, request);

            return new OkResult();
        }

        [HttpGet("sys/get/{id}")]
        public IActionResult GetSys([FromRoute] Guid id)
        {
            var json = sysService.Get(id);

            return new OkObjectResult(json);
        }

        [HttpPost("sys/create/phi/graf")]
        public IActionResult CreateSysPhiGraf([FromQuery] Guid id, [FromQuery] int layersNum)
        {
            var html = sysService.CreatePhiGraf(id, layersNum);

            return new OkObjectResult(html);
        }

        [HttpPost("sys/create/intens/opt/graf")]
        public IActionResult CreateSysIntensOptGraf([FromQuery] Guid id, [FromBody] CreateIntensOptRequest request)
        {
            var html = sysService.CreateIntensOptGraf(id, request);

            return new OkObjectResult(html);
        }
    }
}
