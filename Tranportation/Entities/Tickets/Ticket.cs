using System.Net.Sockets;
using Tranportation.Entities.Enums;
using Tranportation.Entities.Trips;

namespace Tranportation.Entities.Tickets;

public abstract class Ticket
{
    public Ticket()
    {
        
    }

    protected Ticket(string seatNumber, decimal ticketPrice, int tripId)
    {
        SeatNumber = seatNumber;
        TripId = tripId;
        CancelTicket = 0;
        TicketPrice = ticketPrice;
        CancelTicket = 0;
    }
    public int Id { get; set; }
    public int CancelTicket { get; set; }

    public TypeTicket TicketType { get; set; }
    public string SeatNumber { get; set; }
    public decimal Cost { get; set; }
    public decimal TicketPrice { get; set; }

    public Trip Trip { get; set; }
    public int TripId { get; set; }

    public abstract void SetCost(decimal cost);

    public virtual void ChangeCostToGetAllAmount(decimal reserveCost, decimal tripPrice)
    {

        Cost = tripPrice - reserveCost;
    }
   
    
    public void ReturnRestOfAmountOfBookedCalncel()
    {
        Cost = Cost - Cost * 0.2m;
    }
    
    
    public void ReturnRestOfAmountOfSoldCalncel()
    {
        Cost = TicketPrice - TicketPrice * 0.1m;
    }
}
