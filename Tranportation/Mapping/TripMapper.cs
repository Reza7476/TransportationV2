using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tranportation.Entities.Trips;

namespace Tranportation.Mapping;

public class TripMapper : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
   
        builder.HasKey(_ => _.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(_ => _.DepartureFrom)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(_ => _.Destination)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(x => x.Bus)
            .WithMany(x => x.Trips)
            .HasForeignKey(x => x.BusId);
        builder.Property(_=>_.Benefit)
            .HasColumnType("decimal(18,4)");

        builder.Property(_ => _.TripPrice)
            .HasColumnType("decimal(18,4)");

        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<NormalTrip>("NormalTrip")
            .HasValue<VIPTrip>("VipTrip");


    }

   
}
