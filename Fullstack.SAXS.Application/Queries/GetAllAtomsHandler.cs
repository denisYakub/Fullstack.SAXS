using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public class GetAllAtomsHandler(ISpService spService) : IRequestHandler<GetAllAtomsQuery, byte[]>
    {
        public async Task<byte[]> Handle(GetAllAtomsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Shouldn't be null.");

            var json = await spService
                .GetAtomsAsync(request.AreaId)
                .ConfigureAwait(false);

            return json;
        }
    }
}
