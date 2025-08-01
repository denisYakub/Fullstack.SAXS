using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public class CreateIntenseOptGraphHandler : IRequestHandler<CreateIntenseOptGraphCommand, string>
    {
        public Task<string> Handle(CreateIntenseOptGraphCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
