using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public record GetAllAtomsQuery(Guid AreaId) : IRequest<byte[]>;
}
