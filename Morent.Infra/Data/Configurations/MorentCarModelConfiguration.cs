using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infra.Data.Configurations;

public class MorentCarModelConfiguration : IEntityTypeConfiguration<MorentCarModel>
{
  public void Configure(EntityTypeBuilder<MorentCarModel> builder)
  {
    int i = 1;
    builder.Property(c => c.Id)
        .ValueGeneratedOnAdd();

    builder.HasData(
      new MorentCarModel
      {
        Id = i++,
        Capacity = 2,
        Brand = "",
        Model = "Koenigsegg",
        CarTypeId = 1,
        SteeringType = "Manual",
        FuelCapacityLitter = 90,
        PricePerDay = 99,
      },
      new MorentCarModel
      {
        Id = i++,
        Capacity = 2,
        Brand = "Nissan",
        Model = "Nissan GT-R",
        CarTypeId = 1,
        SteeringType = "Manual",
        FuelCapacityLitter = 80,
        PricePerDay = 100,
      },
      new MorentCarModel
      {
        Id = i++,
        Capacity = 4,
        Brand = "Rolls-Royce",
        Model = "Rolls-Royce",
        CarTypeId = 1,
        SteeringType = "Manual",
        FuelCapacityLitter = 70,
        PricePerDay = 96,
      },
      new MorentCarModel
      {
        Id = i++,
        Capacity = 6,
        Brand = "",
        Model = "All New Rush",
        CarTypeId = 2,
        SteeringType = "Manual",
        FuelCapacityLitter = 70,
        PricePerDay = 80,
      },
      new MorentCarModel
      {
        Id = i++,
        Capacity = 6,
        Brand = "",
        Model = "CR-V",
        CarTypeId = 2,
        SteeringType = "Manual",
        FuelCapacityLitter = 80,
        PricePerDay = 80,
      },
      new MorentCarModel
      {
        Id = i++,
        Capacity = 6,
        Brand = "",
        Model = "All New Terios",
        CarTypeId = 2,
        SteeringType = "Manual",
        FuelCapacityLitter = 90,
        PricePerDay = 72,
      },
      new MorentCarModel
      {
        Id = i++,
        Capacity = 4,
        Brand = "",
        Model = "MG ZX Exclusive",
        CarTypeId = 6,
        SteeringType = "Electric",
        FuelCapacityLitter = 70,
        PricePerDay = 80,
      },
      new MorentCarModel
      {
        Id = i++,
        Capacity = 6,
        Brand = "",
        Model = "New MG ZS",
        CarTypeId = 2,
        SteeringType = "Manual",
        FuelCapacityLitter = 80,
        PricePerDay = 80,
      },
      new MorentCarModel
      {
        Id = i++,
        Capacity = 4,
        Brand = "",
        Model = "MG ZX Excite",
        CarTypeId = 6,
        SteeringType = "Electric",
        FuelCapacityLitter = 90,
        PricePerDay = 74,
      }
    );
  }
}
