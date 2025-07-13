using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public record GetSystemQuery(Guid AreaId) : IRequest<string>;
}
