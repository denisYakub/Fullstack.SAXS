using System.Text.Json;
using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public class GetSystemHandler(IStorage storage) : IRequestHandler<GetSystemQuery, string>
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true,
        };

        public async Task<string> Handle(GetSystemQuery request, CancellationToken cancellationToken)
        {
            var area = await storage
                .GetAreaAsync(request.AreaId)
                .ConfigureAwait(false);

            return JsonSerializer
                .Serialize(area, _jsonOptions);
        }
    }
}
