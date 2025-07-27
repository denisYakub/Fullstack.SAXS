using System.Text.Json;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Models;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public class CreateSystemHandler(ISysService sysService, IProducer<SystemMessage> producer) : IRequestHandler<CreateSystemCommand>
    {
        public async Task Handle(CreateSystemCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Shouldn't be null.");

            var dto = new CreateSysData(
                request.Data.AreaSize, request.Data.AreaNumber, request.Data.ParticleNumber,
                request.Data.ParticleType,
                request.Data.ParticleMinSize, request.Data.ParticleMaxSize,
                request.Data.ParticleSizeShape, request.Data.ParticleSizeScale,
                request.Data.ParticleAlphaRotation,
                request.Data.ParticleBetaRotation,
                request.Data.ParticleGammaRotation
            );

            await producer.ProduceAsync(
                new SystemMessage()
                {
                    UserId = request.UserId,
                    CreateSysData = dto
                }
            );

            /*await sysService
                .CreateAsync(request.UserId, request.Data)
                .ConfigureAwait(false);*/
        }
    }
}
