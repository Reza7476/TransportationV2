using System.Net.Sockets;
using Tranportation.Entities.Buses;
using Tranportation.Entities.Enums;
using Tranportation.Entities.Seats;
using Tranportation.Entities.Tickets;

namespace Tranportation.Entities.Trips;

public abstract class Trip
{
    protected Trip(string departureFrom, string destination,decimal tripPrice,  int busId)
    {
        DepartureFrom = departureFrom;
        Destination = destination;
        TripPrice = SetTripPrice(tripPrice);
        EmptySeat = Capacity;
        BusId = busId;
        Tickets = new();
        Seats=new();
    }

    public int Id { get; set; }

    public BusType Type { get; set; }

    public string DepartureFrom { get; set; }

    public string Destination { get; set; }

    public decimal TripPrice { get; private set; }

    public int EmptySeat { get; set; }

    public int Capacity { get; set; }

    public decimal Benefit { get; set; }

    public List<Ticket> Tickets { get; set; }

    public List<Seat> Seats { get; set; }

    public Bus Bus { get; set; }

    public int BusId { get; set; }

    public abstract void SetSeatNumber(int capacity);


    public virtual void DecreasNumberOfEmptySeat()
    {
        EmptySeat -= 1;
        Capacity-= 1;
    }



    public  void IncreaseNumberOfEmptySeats()
    {
        EmptySeat += 1;
        Capacity += 1;
    }
    public abstract void ChangeSetStatusFromEmptyToNext(string status);

    public abstract decimal SetTripPrice(decimal price);
   
    public virtual void SetBenefit(decimal benefit)
    {
        Benefit = Benefit + benefit;
        
    }   
    
    public void DecreaseBenefit(decimal newBenefit)
    {
        Benefit = Benefit- newBenefit;
    }

  

}
