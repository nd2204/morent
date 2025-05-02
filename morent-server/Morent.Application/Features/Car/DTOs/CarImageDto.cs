using System;

namespace Morent.Application.Features.Car.DTOs;

public class CarImageDto
{
    public Guid ImageId { get; set; }
    public string Url { get; set; } = null!;
    public bool IsPrimary { get; set; }
    public int DisplayOrder { get; set; }
}
