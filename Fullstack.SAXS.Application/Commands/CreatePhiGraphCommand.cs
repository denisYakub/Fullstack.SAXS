using Fullstack.SAXS.Domain.Models;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public record CreatePhiGraphCommand(CreateDensityGraphModel model) : IRequest<string>;
}
