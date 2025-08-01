using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public class GetAtomsTableHandler(IStorage storage, IFileService file) : IRequestHandler<GetAtomsTableQuery, byte[]>
    {
        public async Task<byte[]> Handle(GetAtomsTableQuery request, CancellationToken cancellationToken)
        {
            var area = await storage
                .GetAreaAsync(request.AreaId)
                .ConfigureAwait(false);

            return file.SaveAtoms(area);
        }
    }
}
