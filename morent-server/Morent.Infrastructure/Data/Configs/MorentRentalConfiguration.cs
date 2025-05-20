
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

    builder.HasOne(p => p.Car)
      .WithMany()
      .HasForeignKey(e => e.CarId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne<MorentUser>()
      .WithMany()
      .HasForeignKey(e => e.UserId)
      .OnDelete(DeleteBehavior.Restrict);

    // Configure Money Value Object for Cost
    builder.OwnsOne(r => r.TotalCost, cost =>
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
    builder.OwnsOne(r => r.RentalPeriod, r =>
    {
      r.Property(d => d.Start)
      .IsRequired();

      r.Property(d => d.End)
      .IsRequired();
    });

    builder.OwnsOne(r => r.PickupLocation, location =>
    {
      location.Property(l => l.Address).HasColumnName("PickupLocationAddress").IsRequired(false);
      location.Property(l => l.City).HasColumnName("PickupLocationCity").IsRequired(false);
      location.Property(l => l.Country).HasColumnName("PickupLocationCountry").IsRequired(false);
      location.Property(l => l.Longitude).HasColumnName("PickupLocationLongitude");
      location.Property(l => l.Latitude).HasColumnName("PickupLocationLatitude");
    });
    builder.OwnsOne(r => r.DropoffLocation, location =>
    {
      location.Property(l => l.Address).HasColumnName("DropoffLocationAddress").IsRequired(false);
      location.Property(l => l.City).HasColumnName("DropoffLocationCity").IsRequired(false);
      location.Property(l => l.Country).HasColumnName("DropoffLocationCountry").IsRequired(false);
      location.Property(l => l.Longitude).HasColumnName("DropoffLocationLongitude");
      location.Property(l => l.Latitude).HasColumnName("DropoffLocationLatitude");
    });
  }
}

