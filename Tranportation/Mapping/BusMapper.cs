using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tranportation.Entities.Buses;

namespace Tranportation.Mapping;

public class BusMapper : IEntityTypeConfiguration<Bus>
{
    public void Configure(EntityTypeBuilder<Bus> builder)
    {

   

        builder.HasKey(x => x.Id);

        builder.Property(_ => _.Id).ValueGeneratedOnAdd();

        builder.Property(_ => _.Name)
        .HasMaxLength(100);

        builder.HasDiscriminator<string>("Discirminator")
             .HasValue<VIPBus>("VIPBus")
             .HasValue<NormalBus>("Normal");


    }
}
