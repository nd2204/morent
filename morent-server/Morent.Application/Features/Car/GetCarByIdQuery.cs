namespace Morent.Application.Features.Car;

public class GetCarByIdQuery : IQuery<CarDto>
{
  public Guid Id { get; set; }
}
