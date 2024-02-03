using System.Diagnostics;
using Tranportation.Entities.Enums;

namespace Tranportation.Entities.Tickets;

public class BuyTicket : Ticket
{

    public BuyTicket()
    {

    }
    public BuyTicket(string seatNumber, decimal ticketPrice, int tripId) : base(seatNumber, ticketPrice, tripId)
    {
        TicketType = TypeTicket.Buy;
        SetCost(ticketPrice);
    }


    public override void SetCost(decimal ticketPrice)
    {
        Cost = ticketPrice;
    }

   public void GetReturnAmount()
    {

    }
}
