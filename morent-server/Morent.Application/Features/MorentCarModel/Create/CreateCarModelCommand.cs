using System;
using MediatR;
using Morent.Application.Features.MorentCarModel;

namespace Morent.Application.Features.CarModels.Create;

public record CreateCarModelCommand(
  string Brand
) : IRequest<MorentCarModelDto>;