using System.Security.Claims;
using Fullstack.SAXS.Application.Queries;
using Fullstack.SAXS.Application.Services;
using Fullstack.SAXS.Server.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.SAXS.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MathcadController(IMediator mediator) : ControllerBase
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

        [HttpGet("systems/{id}/atoms-coordinates")]
        public async Task<IActionResult> GetCoordinatesOfAtoms([FromRoute] Guid id)
        {
            var csv = await mediator.Send(new GetAllAtomsQuery(id));

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
