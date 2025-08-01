﻿using Fullstack.SAXS.Domain.Dtos;
using MediatR;

namespace Fullstack.SAXS.Application.Commands
{
    public record CreateIntenseOptGraphCommand(IntensityCreateDTO Dto) : IRequest<string>;
}
