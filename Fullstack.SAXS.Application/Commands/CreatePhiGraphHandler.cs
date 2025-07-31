using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public class CreatePhiGraphHandler : IRequestHandler<CreatePhiGraphCommand, string>
    {
        public Task<string> Handle(CreatePhiGraphCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
