using System.Security.Claims;
using Fullstack.SAXS.Application;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Server.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.SAXS.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MathcadController(ISpService sp) : ControllerBase
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

        [HttpGet("systems/{id}/atoms-coordinates")]
        public async Task<IActionResult> GetCoordinatesOfAtoms([FromRoute] Guid id)
        {
            var csv = await sp.GetAtomsAsync(id);

            return File(csv, "text/csv", "vertices.csv");
        }

        [HttpPost("q-values")]
        public IActionResult GetQI([FromBody] CreateIntensOptRequest request)
        {
            var qI = SysService.CreateQs(request.QMin, request.QMax, request.QNum);

            return new OkObjectResult(qI);
        }
    }
}
