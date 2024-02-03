using Tranportation.Entities.Enums;

namespace Tranportation.Entities.Buses;

public class VIPBus : Bus
{
    public VIPBus(string name) : base(name)
    {
        Capacity = 30;
        TypeBus = BusType.VIP;
    }
}
