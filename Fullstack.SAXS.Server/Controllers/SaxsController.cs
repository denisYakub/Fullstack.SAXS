﻿using System.Security.Claims;
using Fullstack.SAXS.Application.Commands;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Application.Queries;
using Fullstack.SAXS.Domain.Models;
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
            var dto = new CreateSysData(
                request.AreaSize, request.AreaNumber, request.ParticleNumber,
                request.ParticleType,
                request.ParticleMinSize, request.ParticleMaxSize,
                request.ParticleSizeShape, request.ParticleSizeScale,
                request.ParticleAlphaRotation,
                request.ParticleBetaRotation,
                request.ParticleGammaRotation
            );

            await mediator.Send(new CreateSystemCommand(UserId, dto));  

            return new OkResult();
        }

        [HttpGet("systems/{id}")]
        public async Task<IActionResult> GetSys([FromRoute] Guid id)
        {
            var json = await mediator.Send(new GetSystemQuery(id));

            return new OkObjectResult(json);
        }

        [HttpPost("systems/{id}/graphs/phi")]
        public async Task<IActionResult> CreateSysPhiGraph([FromRoute] Guid id, [FromQuery] int layersNum = 5)
        {
            var html = await mediator.Send(new CreatePhiGraphCommand(UserId, id, layersNum));

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
            var html = await mediator.Send(new CreateIntenseOptGraphCommand(id, dto));

            return new ContentResult() { Content = html, ContentType = "text/html" };
        }

        [HttpGet("generations")]
        public async Task<IActionResult> GetGenerations()
        {
            var json = await mediator.Send(new GetAllGenerationsQuery());

            return new OkObjectResult(json);
        }

        [HttpGet("users/me/generations")]
        public async Task<IActionResult> GetMyGenerations()
        {
            var json = await mediator.Send(new GetAllGenerationsQuery(UserId));

            return new OkObjectResult(json);
        }
    }
}
