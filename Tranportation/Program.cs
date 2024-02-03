using Tranportation;
using Tranportation.Entities.Enums;


while (true)
{

    try
    {
        Console.WriteLine($"\tMenu:  M \t Exit:  E");
        var a = Console.ReadKey(true);
        int run = Terminal.Run(a);
        switch (run)
        {
            case 8:
                {
                    Environment.Exit(0);
                    break;
                }
            case 1:
                {

                    var busName = Terminal.GetString("\tenter bus name\t"); 
                        
                    var selectTyp = Terminal.GetInteger("select type of bus\n" +
                        "\t 1:   VIP\t 2:  Normal");

                    if (selectTyp == 1)
                    {

                        Terminal.IntroduceBus(busName, BusType.VIP);
                    }

                    else if (selectTyp == 2)
                    {
                        Terminal.IntroduceBus(busName);

                    }
                    else
                    {
                        throw new Exception("invalid input");
                    }
                    break;
                }

            case 2:
                {
                    var getTripType = Terminal.GetInteger("select type of trip\n" +
                        "\t1:  VIP \t2:  Normal");
                    int busId;
                    if (getTripType == 1)
                    {
                        Terminal.GetBuses(BusType.VIP);
                        busId = Terminal.GetInteger("enter bus number according to list");
                        var getDepartureFrom = Terminal.GetString("Enter the origin of the trip");
                        var getDestination = Terminal.GetString("Enter the travel destination");
                        var getPrice = Terminal.GetInteger("enter Vip Price of trip");
                        Terminal.IntroduceTrip(getDepartureFrom, getPrice, getDestination, busId);

                    }
                    else if (getTripType == 2)
                    {
                        Terminal.GetBuses(BusType.Normal);
                        busId = Terminal.GetInteger("enter bus number accordin to list");
                        var getDepartureFrom = Terminal.GetString("Enter the origin of the trip");
                        var getDestination = Terminal.GetString("Enter the travel destination");
                        var getPrice = Terminal.GetInteger("enter Normal Price of trip");
                        Terminal.IntroduceTrip(getDepartureFrom, getDestination, getPrice, busId);
                    }
                    else
                    {
                        throw new Exception("invalid input");
                    }
                    break;
                }
            case 3:
                {

                    Terminal.DisplayTrips();
                    var getTripNumber = Terminal.GetInteger("enter number of trip ");
                    Terminal.GetTripById(getTripNumber);
                    break;
                }

            case 4:
                {
                    Terminal.DisplayTrips();
                    var getTripNumber = Terminal.GetInteger("enter number of trip ");
                    Terminal.GetTripById(getTripNumber);
                    var getSeatNumber = Terminal.GetInteger("enter seatNumber");
                    Terminal.ReserveTicket(getTripNumber, getSeatNumber);

                    break;
                }
            case 5:
                {

                    Terminal.DisplayTrips();
                    var getTripNumber = Terminal.GetInteger("enter number of trip ");
                    Terminal.GetTripById(getTripNumber);
                    var getSeatNumber = Terminal.GetInteger("enter seatNumber");
                    Terminal.BuyTicket(getTripNumber, getSeatNumber);

                    break;
                }

            case 6:
                {
                    Terminal.DisplayTrips();
                    var getTripNumber = Terminal.GetInteger("enter number of trip ");
                    Terminal.GetTripById(getTripNumber);
                    var getSeatNumber = Terminal.GetInteger("enter seatNumber");

                    Terminal.CancelTicket(getTripNumber, getSeatNumber);
                    break;
                }

            case 7:
                {
                    Terminal.Report();
                    break;
                }

        }

    }
    catch (Exception ex)
    {
        Terminal.Error(ex.Message);
    }
}