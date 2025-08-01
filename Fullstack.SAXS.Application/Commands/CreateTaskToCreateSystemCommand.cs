using Fullstack.SAXS.Domain.Dtos;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public record CreateTaskToCreateSystemCommand(SystemCreateDto Dto) : IRequest;
}
