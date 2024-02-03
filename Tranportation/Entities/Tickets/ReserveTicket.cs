namespace Tranportation.Entities.Tickets;

public class ReserveTicket : Ticket
{


    public ReserveTicket()
    {

    }
    public ReserveTicket(string seatNumber, decimal ticketPrice, int tripId) : base(seatNumber, ticketPrice, tripId)
    {
        SetCost(ticketPrice);

    }





    public override void SetCost(decimal ticketPrice)
    {
        Cost = ticketPrice * 0.3m;
    }

    public override void ChangeCostToGetAllAmount(decimal reserveCost,decimal tripPrice)
    {

        Cost =  tripPrice-reserveCost;
    }



}
