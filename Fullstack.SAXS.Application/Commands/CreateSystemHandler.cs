using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public class CreateSystemHandler(ISysService sysService) : IRequestHandler<CreateSystemCommand>
    {
        public async Task Handle(CreateSystemCommand request, CancellationToken cancellationToken)
        {
            await sysService.CreateAsync(request.UserId, request.Data);
        }
    }
}
