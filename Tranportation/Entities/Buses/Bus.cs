using Tranportation.Entities.Enums;
using Tranportation.Entities.Trips;

namespace Tranportation.Entities.Buses;

public abstract class Bus
{
    protected Bus(string name)
    {
        Name = name;

    }
    public int Id { get; set; }
    public string Name { get; set; }
    public BusType TypeBus { get; set; }
    public int Capacity { get; set; }
    public List<Trip> Trips { get; set; }

}

