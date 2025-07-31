using Fullstack.SAXS.Domain.Models;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public record GetAllGenerationsQuery(GetGenerationsModel model) : IRequest<string>;
}
