using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public class GetAllAtomsHandler(ISpService spService) : IRequestHandler<GetAllAtomsQuery, byte[]>
    {
        public async Task<byte[]> Handle(GetAllAtomsQuery request, CancellationToken cancellationToken)
        {
            var json = await spService.GetAtomsAsync(request.AreaId);

            return json;
        }
    }
}
