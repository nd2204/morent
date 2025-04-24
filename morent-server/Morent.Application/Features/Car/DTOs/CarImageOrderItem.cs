using System;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace Morent.Application.Features.Car.DTOs;

public class CarImageOrderItem
{
  public Guid ImageId { get; set; }
  public int NewOrder { get; set; }
}