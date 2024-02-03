using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tranportation.Entities.Tickets;

namespace Tranportation.Mapping;

public class TicketMapper : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {


        builder.Property(_ => _.Cost).HasColumnType("decimal(18,4)");
        builder.Property(_ => _.TicketPrice).HasColumnType("decimal(18,4)");
        
        builder.HasKey(_ => _.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
      
        builder.HasDiscriminator<string>("Discriminator")
           .HasValue<ReserveTicket>("ReserveTicket")
         .HasValue<BuyTicket>("BuyTicket");


    }
}
