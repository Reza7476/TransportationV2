using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tranportation.Entities.Seats;

namespace Tranportation.Mapping;

public class SeatMapper : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {


    
        builder.HasKey(_ => _.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(_ => _.SeatNumber)
            .HasMaxLength(10);
    }
}
