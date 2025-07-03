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
    public class MathcadController(ISysService sysService, ISpService sp) : ControllerBase
    {
        [HttpGet("sys/{id}/atoms")]
        public IActionResult GetCoordinatesOfAtoms([FromRoute] Guid id)
        {
            var csv = sp.GetAtoms(id);

            return File(csv, "text/csv", "vertices.csv");
        }

        [HttpPost("qI")]
        public IActionResult GetQI([FromBody] CreateIntensOptRequest request)
        {
            if (!request.isLegit)
                throw new BadHttpRequestException("Request is not correct!");

            var qI = ISysService.CreateQs(request.QMin, request.QMax, request.QNum);

            return new OkObjectResult(qI);
        }
    }
}
