using Fullstack.SAXS.Domain.Abstractions.Storages;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public class CreateTaskToCreateSystemHandler(IStorage storage) : IRequestHandler<CreateTaskToCreateSystemCommand>
    {
        public async Task Handle(CreateTaskToCreateSystemCommand request, CancellationToken cancellationToken)
        {
            await storage
                .SaveSystemTaskAsync(request.Dto)
                .ConfigureAwait(false);
        }
    }
}
