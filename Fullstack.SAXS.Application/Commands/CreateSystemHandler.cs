using System.Text.Json;
using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public class CreateSystemHandler(ISysService sysService) : IRequestHandler<CreateSystemCommand>
    {
        public async Task Handle(CreateSystemCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Shouldn't be null.");

            await sysService
                .CreateAsync(request.UserId, request.Data)
                .ConfigureAwait(false);

            var eventPayload = JsonSerializer.Serialize(new
            {
                UserId = request.UserId,
                ParticleType = request.Data.particleType,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
