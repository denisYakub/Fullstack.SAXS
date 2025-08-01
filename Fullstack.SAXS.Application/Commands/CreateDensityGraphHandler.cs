using Fullstack.SAXS.Application.Contracts;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public class CreateDensityGraphHandler(
        IDensityService densityService, 
        IStorage storage, 
        IGraphService graph
    ) : IRequestHandler<CreateDensityGraphCommand, string>
    {
        public async Task<string> Handle(CreateDensityGraphCommand request, CancellationToken cancellationToken)
        {
            var area = await storage
                .GetAreaAsync(request.Dto.AreaId)
                .ConfigureAwait(false);

            var (x, y) = await densityService
                .CreatePhiCoordAsync(area, request.Dto.LayersNum)
                .ConfigureAwait(false);

            return await graph
                .GetHtmlPageAsync(x, y, "Layers", "Density", "Phi")
                .ConfigureAwait(false);
        }
    }
}
