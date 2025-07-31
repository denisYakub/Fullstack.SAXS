using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Models;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public class CreateSystemHandler : IRequestHandler<CreateSystemCommand>
    {
        public Task Handle(CreateSystemCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
