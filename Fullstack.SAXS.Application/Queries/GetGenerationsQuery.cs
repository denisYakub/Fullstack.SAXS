using Fullstack.SAXS.Domain.Dtos;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public record GetAllGenerationsQuery(GenerationGetFilterDTO Dto) : IRequest<string>;
}
