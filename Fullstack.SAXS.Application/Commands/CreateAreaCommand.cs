using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public record CreateAreaCommand(string Json) : IRequest;
}
