namespace Morent.Application.Features.MorentCarModel.Create;

public class CreateCarModelCommandHandler : ICommandHandler<CreateCarModelCommand, Result<MorentCarModelDto>>
{
  public Task<Result<MorentCarModelDto>> Handle(CreateCarModelCommand request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}