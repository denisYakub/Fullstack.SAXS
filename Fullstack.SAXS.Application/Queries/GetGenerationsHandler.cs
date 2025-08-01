using System.Reflection;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Dtos;
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
