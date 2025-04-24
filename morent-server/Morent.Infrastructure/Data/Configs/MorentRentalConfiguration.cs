
using Morent.Core.MorentPaymentAggregate;

namespace Morent.Infrastructure.Data.Configs;

public class MorentRentalConfiguration : IEntityTypeConfiguration<MorentRental>
{
  public void Configure(EntityTypeBuilder<MorentRental> builder)
  {
    // Configure Rental builder
    builder.HasKey(e => e.Id);

    builder.HasOne<MorentPayment>()
      .WithOne()
      .HasForeignKey<MorentPayment>(p => p.RentalId);

    builder.HasOne<MorentCar>()
      .WithMany()
      .HasForeignKey(e => e.CarId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne<MorentUser>()
      .WithMany()
      .HasForeignKey(e => e.UserId)
      .OnDelete(DeleteBehavior.Restrict);

    // Configure Money Value Object for Cost
    builder.ComplexProperty(r => r.TotalCost, cost =>
    {
      cost.Property(p => p.Amount)
        .HasColumnName("CostAmount")
        .HasColumnType("decimal(18,2)")
        .IsRequired();

      cost.Property(p => p.Currency)
        .HasColumnName("CostCurrency")
        .HasMaxLength(3)
        .IsRequired();
    });

    // Configure DateRange Value Object
    builder.ComplexProperty(r => r.RentalPeriod, r =>
    {
      r.Property(d => d.Start)
      .IsRequired();

      r.Property(d => d.End)
      .IsRequired();
    });

    // Configure Location Value Objects
    builder.ComplexProperty(r => r.PickupLocation, location =>
    {
      location.Property(l => l.Country)
        .HasColumnName("PickupCountry")
        .HasMaxLength(200)
        .IsRequired();

      location.Property(l => l.Address)
        .HasColumnName("PickupAddress");

      location.Property(l => l.City)
        .HasColumnName("PickupCity");
    });

    builder.ComplexProperty(r => r.DropoffLocation, location =>
    {
      location.Property(l => l.Country)
        .HasColumnName("PickupCountry")
        .HasMaxLength(200)
        .IsRequired();

      location.Property(l => l.Address)
        .HasColumnName("PickupAddress");

      location.Property(l => l.City)
        .HasColumnName("PickupCity");
    });
  }
}

