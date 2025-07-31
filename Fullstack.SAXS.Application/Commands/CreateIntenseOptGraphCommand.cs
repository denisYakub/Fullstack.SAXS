using Fullstack.SAXS.Domain.Models;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public record CreateIntenseOptGraphCommand(CreateIntensityGraphModel model) : IRequest<string>;
}
