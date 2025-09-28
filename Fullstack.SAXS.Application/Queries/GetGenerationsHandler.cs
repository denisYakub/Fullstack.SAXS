using Fullstack.SAXS.Domain.Abstractions.Storages;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public class GetGenerationsHandler(IStorage storage) : IRequestHandler<GetAllGenerationsQuery, string>
    {
        public async Task<string> Handle(GetAllGenerationsQuery request, CancellationToken cancellationToken)
        {
            return await storage
                .GetGenerationsAsync(request.Dto.UserId)
                .ConfigureAwait(false);
        }
    }
}
