using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public class GetSystemHandler : IRequestHandler<GetSystemQuery, string>
    {
        public Task<string> Handle(GetSystemQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
