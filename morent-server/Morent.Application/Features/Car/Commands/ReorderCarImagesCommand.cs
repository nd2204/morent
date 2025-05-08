using System;

namespace Morent.Application.Features.Car.Commands;

public record class ReorderCarImagesCommand(
  Guid CarId, List<CarImageOrderItem> newOrder) : ICommand<Result>;
