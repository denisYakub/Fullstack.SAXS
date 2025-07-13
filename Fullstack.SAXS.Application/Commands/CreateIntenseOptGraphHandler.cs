using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public class CreateIntenseOptGraphHandler(ISysService sysService) : IRequestHandler<CreateIntenseOptGraphCommand, string>
    {
        public async Task<string> Handle(CreateIntenseOptGraphCommand request, CancellationToken cancellationToken)
        {
            var html = await sysService.CreateIntensOptGraphAsync(request.AreaId, request.Data);

            return html;
        }
    }
}
