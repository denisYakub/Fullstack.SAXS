using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public class GetSystemHandler(ISpService spService) : IRequestHandler<GetSystemQuery, string>
    {
        public async Task<string> Handle(GetSystemQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Shouldn't be null.");

            var json = await spService
                .GetAsync(request.AreaId)
                .ConfigureAwait(false);

            return json;
        }
    }
}
