using System.Text.Json;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.Jsons;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public class CreateAreaHandler(ISysService sysService, IStorage storage) : IRequestHandler<CreateAreaCommand>, IMessageHandler<string>
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task Handle(CreateAreaCommand request, CancellationToken cancellationToken)
        {
            await HandleAsync(request.Json, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task HandleAsync(string message, CancellationToken cancellationToken)
        {
            var dto = JsonSerializer.Deserialize<SystemCreateJSON>(message, _options)!;

            Enum.TryParse<TaskState>(dto.State, true, out var taskState);
            Enum.TryParse<AreaTypes>(dto.AreaType, true, out var areaType);
            Enum.TryParse<ParticleTypes>(dto.ParticleType, true, out var particleType);

            var areaDTO = new AreaCreateDTO(
                dto.AreaSize,
                dto.AreaNumber,
                areaType
            );

            var particleDTO = new ParticleCreateDTO(
                dto.ParticleNumber,
                particleType,
                dto.MinSize, dto.MaxSize,
                dto.SizeShape, dto.SizeScale,
                dto.AlphaRotation, dto.BetaRotation, dto.GammaRotation,
                -dto.AreaSize, dto.AreaSize,
                -dto.AreaSize, dto.AreaSize,
                -dto.AreaSize, dto.AreaSize
            );

            var userId = dto.UserId;

            var areas = sysService.Create(areaDTO, particleDTO);

            await storage
                .AddRangeAsync(areas, userId)
                .ConfigureAwait(false);

            await storage
                .UpdateSystemTaskStateAsync(dto.Id, TaskState.Ready)
                .ConfigureAwait(false);
        }
    }
}
