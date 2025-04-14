namespace Morent.Application.Features.MorentCarModel.Create;

public record CreateCarModelCommand(MorentCarModelDto request) : ICommand<Result<MorentCarModelDto>>;