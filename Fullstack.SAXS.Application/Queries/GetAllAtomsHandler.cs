using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public class GetAllAtomsHandler : IRequestHandler<GetAllAtomsQuery, byte[]>
    {
        public Task<byte[]> Handle(GetAllAtomsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
