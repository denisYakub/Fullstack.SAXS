using System.Security.Claims;
using Fullstack.SAXS.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.SAXS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MathcadController(ISysService sysService) : ControllerBase
    {
        [HttpGet("sys/{id}/atoms")]
        public IActionResult GetCoordinatesOfAtoms([FromRoute] Guid id)
        {
            var csv = sysService.GetAtoms(id);

            return File(csv, "text/csv", "vertices.csv");
        }
    }
}
