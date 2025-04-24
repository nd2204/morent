using System;
using Moq;
using Morent.Application.Repositories;
using Morent.Core.MorentCarAggregate;
using Morent.Core.ValueObjects;

namespace Morent.Application.Tests.Mocks;

public class MockCarRepository
{

  public static Mock<ICarRepository> GetCars()
  {
    var cars = new List<MorentCar>
    {
    };

    var mockRepo = new Mock<ICarRepository>();
    return mockRepo;
  }

}
