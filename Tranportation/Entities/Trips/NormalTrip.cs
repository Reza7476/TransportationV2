using System.Diagnostics;
using Tranportation.Entities.Enums;
using Tranportation.Entities.Seats;

namespace Tranportation.Entities.Trips;

public class NormalTrip : Trip
{
    public NormalTrip(string departureFrom,
        string destination,
        decimal tripPrice, 
        int busId) : base(departureFrom, destination, tripPrice, busId)
    {
        Type = BusType.Normal;
        Capacity = 44;
        Seats = new(Capacity);
        EmptySeat = Capacity;
        SetSeatNumber(Capacity);
    }

    public override void ChangeSetStatusFromEmptyToNext(string status)
    {
        throw new NotImplementedException();
    }

    public override void SetBenefit(decimal benefit)
    {
        Benefit= Benefit+benefit;
    }

    public override void SetSeatNumber(int capacity)
    {
        foreach (var item in Enumerable.Range(1, 44))
        {
            if (item < 10)
            {
                Seats.Add(new Seat()
                {
                    SeatNumber = "0" + item.ToString()
                });
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