using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public class GetAllGenerationsHandler(ISpService spService) : IRequestHandler<GetAllGenerationsQuery, string>
    {
        public async Task<string> Handle(GetAllGenerationsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Shouldn't be null.");

            var json = await spService
                .GetAllAsync(request.UserId)
                .ConfigureAwait(false);

            return json;
        }
    }
}
