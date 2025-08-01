using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Models;
using MediatR;

namespace Fullstack.SAXS.Application.Queries
{
    public record GetAllGenerationsQuery(GenerationGetFilterDTO Dto) : IRequest<string>;
}
