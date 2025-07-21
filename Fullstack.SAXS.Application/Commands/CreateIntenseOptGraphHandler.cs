using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public class CreateIntenseOptGraphHandler(ISysService sysService) : IRequestHandler<CreateIntenseOptGraphCommand, string>
    {
        public async Task<string> Handle(CreateIntenseOptGraphCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Shouldn't be null.");

            var html = await sysService
                .CreateIntensOptGraphAsync(request.AreaId, request.Data)
                .ConfigureAwait(false);

            return html;
        }
    }
}
