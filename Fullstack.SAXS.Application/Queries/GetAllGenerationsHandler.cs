using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public class GetAllGenerationsHandler : IRequestHandler<GetAllGenerationsQuery, string>
    {
        public Task<string> Handle(GetAllGenerationsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
