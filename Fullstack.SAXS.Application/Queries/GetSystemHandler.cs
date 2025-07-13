using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public class GetSystemHandler(ISpService spService) : IRequestHandler<GetSystemQuery, string>
    {
        public async Task<string> Handle(GetSystemQuery request, CancellationToken cancellationToken)
        {
            var json = await spService.GetAsync(request.AreaId);

            return json;
        }
    }
}
