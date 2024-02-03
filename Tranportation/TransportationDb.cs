using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Tranportation.Entities.Buses;
using Tranportation.Entities.Seats;
using Tranportation.Entities.Tickets;
using Tranportation.Entities.Trips;

namespace Tranportation;
public class TransportationDb : DbContext
{


    public DbSet<Bus> Buses { get; set; }
    public DbSet<VIPBus> VIPBuses { get; set; }
    public DbSet<NormalBus> NormalBuses { get; set; }


    public DbSet<Trip> Trips { get; set; }
    public DbSet<NormalTrip> NormalTrips { get; set; }
    public DbSet<VIPTrip> VIPTrips { get; set; }
   
    public DbSet<Seat> Seats { get; set; }

    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<ReserveTicket> Reserve { get; set; }
    public DbSet<BuyTicket> Buy { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-9PR0IFL\\SQLREZA;Initial Catalog=TaavDbCodeFirst_Transportation;Integrated Security=True");

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransportationDb).Assembly);
    }
}
