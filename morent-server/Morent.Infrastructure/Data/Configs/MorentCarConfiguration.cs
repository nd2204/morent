using System;
using Morent.Core.ValueObjects;

namespace Morent.Infrastructure.Data.Configs;

public class MorentCarConfiguration : IEntityTypeConfiguration<MorentCar>
{
  public void Configure(EntityTypeBuilder<MorentCar> builder)
  {
    builder.HasKey(e => e.Id);
    builder.HasOne(x => x.CarModel)
      .WithMany()
      .HasForeignKey(x => x.CarModelId);

    // Configure Money Value Object
    builder.ComplexProperty(c => c.PricePerDay, price =>
    {
      price.Property(p => p.Amount)
        .HasColumnName("PriceAmount")
        .HasColumnType("decimal(18,2)")
        .IsRequired();

      price.Property(p => p.Currency)
        .HasColumnName("PriceCurrency")
        .HasMaxLength(3)
        .IsRequired();
    });

    builder.ComplexProperty(c => c.CurrentLocation, c => { c.IsRequired(); });

    // Configure Images collection
    builder.HasMany(e => e.Images)
      .WithOne()
      .HasForeignKey(i => i.CarId);
  }

  // private void SeedCar(EntityTypeBuilder builder)
  // {
  //   var cars = new List<MorentCar>();

  //   cars.AddRange(
  //     MorentCar.Create(
  //       brand: "Toyota",
  //       model: "Camry",
  //       year: 2022,
  //       licensePlate: "TOY1234",
  //       capacity: 5,
  //       fuelType: FuelType.Diesel,
  //       pricePerDay: new Money(49.99m),
  //       currentLocation: new Location("Los Angeles", "123 Sunset Blvd")
  //     ),
  //   );
  //   var car1 = MorentCar.Create(
  //   );

  //   var car2 = MorentCar.Create(
  //       brand: "Tesla",
  //       model: "Model 3",
  //       year: 2023,
  //       licensePlate: "TES1234",
  //       capacity: 5,
  //       fuelType: FuelType.Electric,
  //       pricePerDay: Money.FromDecimal("USD", 89.99m),
  //       currentLocation: new Location("San Francisco", "456 Market St")
  //   );

  //   var car3 = MorentCar.Create(
  //       brand: "Honda",
  //       model: "Civic",
  //       year: 2021,
  //       licensePlate: "HON9876",
  //       capacity: 5,
  //       fuelType: FuelType.Gasoline,
  //       pricePerDay: Money.FromDecimal("USD", 39.99m),
  //       currentLocation: new Location("New York", "789 Broadway")
  //   );

  //   var car4 = MorentCar.Create(
  //       brand: "BMW",
  //       model: "X5",
  //       year: 2020,
  //       licensePlate: "BMW5555",
  //       capacity: 7,
  //       fuelType: FuelType.Diesel,
  //       pricePerDay: Money.FromDecimal("USD", 99.99m),
  //       currentLocation: new Location("Chicago", "101 Lakeshore Drive")
  //   );

  //   var car5 = MorentCar.Create(
  //       brand: "Hyundai",
  //       model: "Tucson",
  //       year: 2022,
  //       licensePlate: "HYU1122",
  //       capacity: 5,
  //       fuelType: FuelType.Hybrid,
  //       pricePerDay: Money("USD", 59.99m),
  //       currentLocation: new Location("Houston", "202 Energy St")
  //   );
  //   builder.HasData(new MorentCar[]
  //   {


  //   });
  // }
}
