using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using Tranportation.Entities.Buses;
using Tranportation.Entities.Enums;
using Tranportation.Entities.Tickets;
using Tranportation.Entities.Trips;

namespace Tranportation;
public class Terminal
{


    static TransportationDb _db = new TransportationDb();

    public static int Run(ConsoleKeyInfo a)
    {
        if (a.Key == ConsoleKey.M || a.Key == ConsoleKey.Escape)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Chose an option\n" +
                "\t 1: Introduce Bus\n" +
                "\t 2: Introduce Trip\n" +
                "\t 3: Display Trips\n" +
                "\t 4: Book Ticket\n " +
                "\t 5: Buy Ticket\n" +
                "\t 6: Cancel the Ticket\n" +
                "\t 7: Get Report\n" +
                "\t 8: Exit");
            Console.ResetColor();
            var value = int.Parse(Console.ReadLine()!);
            return value;
        }
        else
        {
            return 8;
        }
    }

    public static void Report()
    {
        var trips = _db.Trips.ToList();
        foreach (var trip in trips)
        {
            var cancelReserve = trip.Tickets.Where(x => x.TicketType == TypeTicket.reserve && x.CancelTicket == 1).ToList();

            var cancelBuy = trip.Tickets.Where(x => x.TicketType == TypeTicket.Buy && x.CancelTicket == 1).ToList();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\t {trip.DepartureFrom}    {trip.Destination}  ({trip.Type}) \n " +
                $" \t \t Net profit of the trip: {trip.Benefit}\n" +
                $" \t \t Number of empty seats: {trip.EmptySeat}\n" +
                $" \t \t Number of booked seats that have been cancelled.: {cancelReserve.Count}\n" +
                $" \t \t Number of purchased seats that have been cancelled.: {cancelBuy.Count}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-----------------------------------");
            Console.ResetColor();
        }
    }

    public static void CancelTicket(int tripId, int seatNumb)
    {
        string seatNumber = "";
        if (seatNumb < 10)
        {
            seatNumber = "0" + seatNumb.ToString();
        }
        else
        {
            seatNumber = seatNumb.ToString();
        }

        var trip = _db.Trips.FirstOrDefault(x => x.Id == tripId);
        var seat = _db.Seats.Where(_ => _.TripId == tripId).ToList();
        var seatNum = seat.ElementAt(seatNumb - 1);
        var ticket = _db.Tickets
            .FirstOrDefault(x => x.TripId == tripId && x.SeatNumber == seatNumber && x.CancelTicket == 0);
        decimal returnPriceToCustomer = 0m;
        if (seatNum.SeatNumber == "rr")
        {

            var recievedAmount = ticket.Cost;
            ticket.ReturnRestOfAmountOfBookedCalncel();
            ticket.CancelTicket = 1;

            _db.SaveChanges();
            returnPriceToCustomer = ticket.Cost;
            trip.DecreaseBenefit(ticket.Cost);
            trip.IncreaseNumberOfEmptySeats();

            seatNum.SeatNumber = seatNumber;
            _db.SaveChanges();

        }
        else if (seatNum.SeatNumber == "bb")
        {
            ticket.ReturnRestOfAmountOfSoldCalncel();
            ticket.CancelTicket = 1;
            _db.SaveChanges();
            returnPriceToCustomer = ticket.Cost;
            trip.DecreaseBenefit(ticket.Cost);
            trip.IncreaseNumberOfEmptySeats();
            seatNum.SeatNumber = seatNumber;
            _db.SaveChanges();
        }
        else
        {
            throw new Exception("Invalid input");
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\t ticket canceled return amount: {returnPriceToCustomer}");
        Console.ResetColor();

    }


    public static void BuyTicket(int tripId, int seatNumb)
    {
        string seatNumber = "";
        if (seatNumb < 10)
        {
            seatNumber = "0" + seatNumb.ToString();
        }
        else
        {
            seatNumber = seatNumb.ToString();
        }
        var ticket = _db.Reserve.FirstOrDefault(x => x.TripId == tripId && x.SeatNumber == seatNumber);
        var trip = _db.Trips.FirstOrDefault(x => x.Id == tripId);
        var seat = _db.Seats.Where(_ => _.TripId == tripId).ToList();
        var seatNum = seat.ElementAt(seatNumb - 1);
        if (seatNum.SeatNumber == "rr")
        {
            var confirmBuy = GetInteger("this tickt has already been booked chose an option \n" +
                "\t 1:   complete buy ticket\t 2:  Discard");
            switch (confirmBuy)
            {
                case 1:
                    {
                        string amountPaid = ticket.Cost.ToString();
                        BuyBookedTicket(ticket.Id, seatNum.Id, tripId);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($" \t  Ticket Number:{ticket.Id}\n" +
                            $"\t  Seat Number: {ticket.SeatNumber}\n" +
                            $"\t  Ticket Type: {ticket.TicketType}\n" +
                            $"\t  Amount Paid: {amountPaid}\n" +
                            $"\t  Rest Of Amount: {ticket.Cost}");
                        Console.ResetColor();
                        break;
                    }
                case 2:
                    {
                        throw new Exception("chose correct option");
                      
                    }
                default:
                    {
                        throw new Exception("invlaid input");
                    }
            }
        }
        else if (seatNum.SeatNumber == "bb")
        {
            throw new Exception("thos ticket has alreday been sold");
        }
        else
        {
            SaleTicket( seatNumber, tripId, seatNum.Id);
            var SetTicket = _db.Buy.FirstOrDefault(x => x.TripId == tripId && x.SeatNumber == seatNumber&&x.CancelTicket==0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" \t TicketNumber:{SetTicket.Id}\n" +
                $"\t SeatNumber: {SetTicket.SeatNumber}\n" +
                $"\t TicketType: {SetTicket.TicketType}\n" +
                $"\t The amount payable: {SetTicket.TicketPrice}");
            Console.ResetColor();
        }
    }

    public static void SaleTicket(string seatNumber,int tripId ,int seatId)
    {


        var trip = _db.Trips.First(x => x.Id == tripId);
        var seat = _db.Seats.First(x => x.Id == seatId);
        BuyTicket buy = new(seatNumber,trip.TripPrice , tripId);
        _db.Tickets.Add(buy);
        trip.DecreasNumberOfEmptySeat();
        trip.SetBenefit(trip.TripPrice);
        seat.ChangeSeatStatusToBuy();

        _db.SaveChanges();
    }

    public static void BuyBookedTicket(int ticketId,int seatId,int tripId)
    {
        var ticket = _db.Tickets.First(x => x.Id == ticketId);
        var seat=_db.Seats.First(x=>x.Id==seatId);
        seat.ChangeSeatStatusToBuy();
        ticket.ChangeCostToGetAllAmount(ticket.Cost, ticket.TicketPrice);
        ticket.TicketType = TypeTicket.Buy;
        var trip = _db.Trips.First(x => x.Id == tripId);
        trip.SetBenefit(ticket.Cost);
        _db.SaveChanges();
    }
    public static void ReserveTicket(int tripId, int seatNumb)
    {
        string seatNumber = "";
        if (seatNumb < 10)
        {
            seatNumber = "0" + seatNumb.ToString();
        }
        else
        {
            seatNumber = seatNumb.ToString();
        }
        var trip = _db.Trips.FirstOrDefault(x => x.Id == tripId);
        if (trip == null)
        {
            throw new Exception("invalid input");
        }
        var seat = _db.Seats.Where(_ => _.TripId == tripId && _.SeatNumber == seatNumber).FirstOrDefault(); 
        if (seat == null)
        {
            throw new Exception($"this ticket is sold or reserved");
        }
        var tripPrice = trip.TripPrice;
        ReserveTicket reserve = new(seatNumber, trip.TripPrice, tripId);
        _db.Tickets.Add(reserve);
        trip.DecreasNumberOfEmptySeat();
        seat.ChangeSeatStatusTobook();
        _db.SaveChanges();
        var ticket = trip.Tickets.Find(x => x.SeatNumber == seatNumber);
        trip.SetBenefit(ticket!.Cost);
        _db.SaveChanges();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\t   ticketNumber: {ticket.Id} \n" +
            $"\t   SeatNumber: {ticket.SeatNumber}\n" +
            $"\t   TicketType: {ticket.TicketType} \n" +
            $"\t   The amount payable: {ticket.Cost}\n" +
            $"\t   Rest Of Amount: {ticket.TicketPrice - ticket.Cost}");
        Console.ResetColor();
    }

    public static void GetTripById(int tripNumber)
    {
        var trip = _db.Trips.FirstOrDefault(_ => _.Id == tripNumber);
        if (trip == null)
        {
            throw new Exception("trip is not defind");
        }
        else
        {
            DisplayTripsSeats(trip, tripNumber);
        }
    }

    public static void DisplayTrips()
    {

        var vipTrips = _db.VIPTrips.OfType<VIPTrip>().ToList();
        if (vipTrips.Count > 0)
        {

            foreach (var vip in vipTrips)
            {
                var busName = _db.VIPBuses.OfType<VIPBus>().FirstOrDefault(x => x.Id == vip.BusId);
                Console.WriteLine($" {vip.Id} " +
                    $"BusName:{vip.Bus.Name} " +
                    $"Type Of Trip: {vip.Bus.TypeBus} " +
                    $"DepartureFrom:  {vip.DepartureFrom} " +
                    $"Destination:  {vip.Destination} " +
                    $"Price:  {vip.TripPrice}" +
                    $" EmptySeat: {vip.EmptySeat}");
            }
        }
        var normalTrips = _db.NormalTrips.OfType<NormalTrip>().ToList();

        if (normalTrips.Count > 0)
        {
            foreach (var normal in normalTrips)
            {
                var busName = _db.NormalBuses.OfType<NormalBus>().FirstOrDefault(x => x.Id == normal.BusId);
                Console.WriteLine($" {normal.Id} " +
                   $"BusName:{normal.Bus.Name} " +
                   $"Type Of Trip: {normal.Bus.TypeBus} " +
                   $"DepartureFrom:  {normal.DepartureFrom} " +
                   $"Destination:  {normal.Destination} " +
                   $"Price:  {normal.TripPrice}" +
                   $" EmptySeat: {normal.EmptySeat}");
            }
        }
    }

    public static void DisplayTripsSeats<T>(T TripTyp, int tripId)
    {
        Console.WriteLine();
        if (TripTyp is VIPTrip)
        {


            var vip = _db.Seats.Where(x => x.TripId == tripId).ToList();
            for (int row = 1; row <= 30; row += 3)
            {
                if (row != 16)
                {
                    Console.Write($"{vip[row - 1].SeatNumber}  {vip[row].SeatNumber} {vip[row + 1].SeatNumber}");
                }
                else
                {
                    Console.WriteLine($"{vip[row - 1].SeatNumber}");
                    Console.WriteLine($"{vip[row].SeatNumber}");
                    Console.Write($"{vip[row + 1].SeatNumber}");
                }
                Console.WriteLine();
            }
        }
        else
        {
            if (TripTyp is NormalTrip)
            {
                var normal = _db.Seats.Where(x => x.TripId == tripId).ToList();
                for (int row = 2; row <= 44; row += 4)
                {
                    if (row != 22)
                    {
                        Console.Write($"{normal[row - 2].SeatNumber} {normal[row - 1].SeatNumber}     {normal[row].SeatNumber} {normal[row + 1].SeatNumber}");
                    }
                    else
                    {
                        Console.WriteLine($"{normal[row - 2].SeatNumber} {normal[row - 1].SeatNumber}");
                        Console.Write($"{normal[row].SeatNumber} {normal[row + 1].SeatNumber}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }

    //vip
    public static void IntroduceTrip(string destination, decimal tripPrice, string departureFrom, int busId)
    {

        var bus = _db.Buses.FirstOrDefault(s => s.Id == busId);
        if (bus == null)
        {
            throw new Exception("invalid input");
        }


        VIPTrip trip = new(departureFrom, destination, tripPrice, busId);

        _db.VIPTrips.Add(trip);
        _db.SaveChanges();
        Success();
    }
    //normal
    public static void IntroduceTrip(string departureFrom, string destination, decimal tripPrice, int busId)

    {

        var bus = _db.Buses.FirstOrDefault(s => s.Id == busId);
        if (bus == null)
        {
            throw new Exception("invalid input");
        }



        NormalTrip trip = new(departureFrom, destination, tripPrice, busId);

        _db.NormalTrips.Add(trip);
        _db.SaveChanges();
        Success();
    }

    public static void GetBuses(BusType busType)
    {
        Console.WriteLine();
        if (busType == BusType.VIP)
        {
            var bus = _db.Buses.OfType<VIPBus>().ToList();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t VIP Buses");
            foreach (var vip in bus)
            {
                Console.WriteLine($"\t{vip.Id}    {vip.Name}");
            }
            Console.ResetColor();
        }
        if (busType == BusType.Normal)
        {
            var bus = _db.Buses.OfType<NormalBus>().ToList();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\tNormal Buses");
            foreach (var normal in bus)
            {
                Console.WriteLine($"\t{normal.Id}    {normal.Name}");
            }
            Console.ResetColor();
        }
    }

    public static string GetString(string message)
    {
        Console.WriteLine();
        Console.WriteLine($"\t {message}");
        bool exe = true;
        string vlue = "";
        do
        {
            ConsoleKeyInfo readKey = Console.ReadKey(true);
            if (readKey.Key == ConsoleKey.Escape)
            {
                exe = false;
                Run(readKey);
            }
            else
            {
                if (readKey.Key == ConsoleKey.Enter)
                {
                    exe = false;
                    return vlue;
                }
                vlue += readKey.KeyChar;
                Console.Write(readKey.KeyChar);
            }
            // vlue = "";
        } while (exe);
        return vlue;
    }

    public static int GetInteger(string message)
    {
        Console.WriteLine();
        Console.WriteLine($"\t {message}");

        bool exe = true;
        string vlue = "";
        do
        {
            ConsoleKeyInfo readKey = Console.ReadKey(true);
            if (readKey.Key == ConsoleKey.Escape)
            {
                exe = false;
                Run(readKey);
            }
            else
            {
                if (readKey.Key == ConsoleKey.Enter)
                {
                    Console.Write(readKey.KeyChar);
                    return int.Parse(vlue);
                }
                else
                {
                    vlue += readKey.KeyChar;
                    Console.Write(readKey.KeyChar);
                }

            }

        } while (exe);
        Console.WriteLine();
        return int.Parse(vlue);




    }
    //vip
    public static void IntroduceBus(string busName, BusType busType)
    {

        VIPBus vipBus = new VIPBus(busName);
        _db.VIPBuses.Add(vipBus);
        _db.SaveChanges();
        Success();
    }
    //normal
    public static void IntroduceBus(string busName)
    {
        NormalBus vipBus = new NormalBus(busName);
        _db.NormalBuses.Add(vipBus);
        _db.SaveChanges();
        Success();
    }

    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\t {message}");
        Console.ResetColor();
    }

    public static void Success()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\t \t Succecced");
        Console.ResetColor();
    }

}
