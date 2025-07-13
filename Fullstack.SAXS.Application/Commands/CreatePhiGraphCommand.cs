using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public record CreatePhiGraphCommand(Guid UserId, Guid AreaId, int LayersNumber) : IRequest<string>;
}
