using System.Diagnostics;
using System.Runtime.CompilerServices;
using Tranportation.Entities.Enums;
using Tranportation.Entities.Seats;

namespace Tranportation.Entities.Trips;

public class VIPTrip : Trip
{

    public VIPTrip(string departureFrom,
        string destination,
        decimal tripPrice,
        int busId) : base(departureFrom, destination,tripPrice,busId)

    {
        Type = BusType.VIP;
        Capacity = 30;
        Seats = new(Capacity);
        SetSeatNumber(Capacity);
        EmptySeat = Capacity;

    }

    public override void ChangeSetStatusFromEmptyToNext(string status)
    {
        throw new NotImplementedException();
    }

    public override void SetBenefit(decimal benefit)
    {
        Benefit = Benefit + benefit;
    }

    public override void SetSeatNumber(int capacity)
    {

        foreach (var item in Enumerable.Range(1,30))
        {
            if (item < 10)
            {
                Seats.Add(new Seat()
                {
                    SeatNumber = "0" + item.ToString()
                }) ;
            }
            else
            {
                Seats.Add(new Seat()
                {
                    SeatNumber = item.ToString()
                });
            }
        }
    }

    public override decimal SetTripPrice(decimal price)
    {
        if (price < 0)
        {
            throw new Exception("proce can not be less than zero");
        }
        return price;
    }
}
