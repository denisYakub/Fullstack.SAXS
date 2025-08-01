using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public record GetAtomsTableQuery(Guid AreaId) : IRequest<byte[]>;
}
