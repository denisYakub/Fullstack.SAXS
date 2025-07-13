using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.Models;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public record CreateSystemCommand(Guid UserId, CreateSysData Data) : IRequest;
}
