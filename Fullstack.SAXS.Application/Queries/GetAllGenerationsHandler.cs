using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public class GetAllGenerationsHandler(ISpService spService) : IRequestHandler<GetAllGenerationsQuery, string>
    {
        public async Task<string> Handle(GetAllGenerationsQuery request, CancellationToken cancellationToken)
        {
            var json = await spService.GetAllAsync(request.UserId);

            return json;
        }
    }
}
