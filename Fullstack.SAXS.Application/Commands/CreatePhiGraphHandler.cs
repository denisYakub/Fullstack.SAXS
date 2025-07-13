using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public class CreatePhiGraphHandler(ISysService sysService) : IRequestHandler<CreatePhiGraphCommand, string>
    {
        public async Task<string> Handle(CreatePhiGraphCommand request, CancellationToken cancellationToken)
        {
            var html = await sysService.CreatePhiGraphAsync(request.UserId, request.AreaId, request.LayersNumber);

            return html;
        }
    }
}
