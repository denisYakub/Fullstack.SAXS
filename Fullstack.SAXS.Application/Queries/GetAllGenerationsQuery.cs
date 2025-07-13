using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public record GetAllGenerationsQuery(Guid? UserId = null) : IRequest<string>;
}
