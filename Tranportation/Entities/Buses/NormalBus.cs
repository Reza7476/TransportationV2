using Tranportation.Entities.Enums;

namespace Tranportation.Entities.Buses;

public class NormalBus : Bus
{
    public NormalBus(string name) : base(name)
    {
        Capacity = 44;
        TypeBus = BusType.Normal;
    }

}
