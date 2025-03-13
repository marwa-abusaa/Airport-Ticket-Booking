using Airport_Ticket_Booking.Domain.FlightManagement;
using Airport_Ticket_Booking.Domain.General;
using Airport_Ticket_Booking.Domain.Models;
using Airport_Ticket_Booking.Domain.Records;
using Airport_Ticket_Booking.Services;
using System.Globalization;
using Airport_Ticket_Booking.Storage;

namespace Airport_Ticket_Booking.Presentation.Menu;

public class PassengerMenu
{
    public static void PassengersMenu()
    {
        Console.WriteLine("\n1. Search Available Flights");
        Console.WriteLine("2. Book a Flight");
        Console.WriteLine("3. View Personal Bookings");
        Console.WriteLine("4. Modify a Booking");
        Console.WriteLine("5. Cancel a Booking");
        Console.WriteLine("6. View All Available Flights");
        Console.WriteLine("7. Exit");
        Console.Write("\nSelect an option: ");
    }

    public static void PassengerActions()
    {

        FileHandler fileHandler = new FileHandler();
        BookingMap bookingMap = new BookingMap(fileHandler);
        FlightMap flightMap = new FlightMap(fileHandler);
        BookingService bookingService = new BookingService(bookingMap);
        PassengerService passengerService = new PassengerService(bookingService, flightMap);
        CriteriaSearch criteriaSearch = new CriteriaSearch();

        Console.WriteLine("Welcome! What would you like to do?");

        while (true)
        {
            PassengersMenu();

            switch (Console.ReadLine())
            {
                case "1":
                    SearchAvailableFlights(criteriaSearch, passengerService);
                    break;
                case "2":
                    BookFlight(passengerService, flightMap);
                    break;
                case "3":
                    ViewPersonalBookings(passengerService);
                    break;
                case "4":
                    ModifyBooking(passengerService);
                    break;
                case "5":
                    CancelBooking(passengerService);
                    break;
                case "6":
                    ViewAllAvailableFlights(flightMap);
                    break;
                case "7":
                    return;
                default:
                    {
                        Console.WriteLine("Please, choose a valid number!\n");
                        break;
                    }

            }
        }
    }

    public static void Display<T>(List<T> list)
    {
        if (typeof(T) == typeof(Flight))
            Console.WriteLine("\nFlightNumber DepartureCountry DestinationCountry DepartureDate DepartureAirport ArrivalAirport Class Price\n");

        else if (typeof(T) == typeof(Booking))
            Console.WriteLine("\nBookingId FlightId Class Price\n");

        foreach (var l in list)
            Console.WriteLine(l);
    }

    public static void SearchAvailableFlights(CriteriaSearch criteria, PassengerService passengerService)
    {
        Console.Write("For each field, enter a value or press ENTER to skip and leave it empty.\n");

        Console.Write("Enter Departure Country: ");
        var departureCountry = Console.ReadLine();

        Console.Write("Enter Destination Country: ");
        var destinationCountry = Console.ReadLine();

        Console.Write("Enter Departure Airport: ");
        var departureAirport = Console.ReadLine();

        Console.Write("Enter Arrival Airport: ");
        var arrivalAirport = Console.ReadLine();

        DateTime? departureDate = null;
        while (true)
        {
            Console.Write("Enter Departure Date (yyyy-MM-dd): ");
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                departureDate = null;
                break;
            }

            else if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                departureDate = parsedDate;
                break;
            }

            Console.WriteLine("Invalid date format.");
        }

        string? Class;
        while (true)
        {
            Console.Write("Enter Flight Class (Economy, Business, First_Class): ");
            Class = Console.ReadLine();
            if (string.IsNullOrEmpty(Class) || Enum.IsDefined(typeof(FlightClassType), Class))
                break;
            Console.WriteLine("Invalid flight class.");
        }

        decimal? maxPrice = null;
        while (true)
        {
            Console.Write("Enter maximum price: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                maxPrice = null;
                break;
            }

            else if (decimal.TryParse(input, out decimal parsedPrice) && parsedPrice > 0)
            {
                maxPrice = parsedPrice;
                break;
            }
            Console.WriteLine("Invalid input! Please enter a number.");
        }

        criteria = new CriteriaSearch(
            departureCountry,
            destinationCountry,
            departureDate,
            departureAirport,
            arrivalAirport,
            Class,
            maxPrice
            );

        var flights = passengerService.SearchAvailableFlights(criteria);
        if (flights != null)
        {
            Display(flights);
        }
        else
            Console.WriteLine("No flights found.");
    }

    public static void BookFlight(PassengerService passengerService, FlightMap flightMap)
    {
        var allFlights = flightMap.GetAllFlights();
        if (!allFlights.Any())
        {
            Console.WriteLine("No flights available");
        }
        else
        {
            Display(allFlights);
            int id;
            while (true)
            {
                Console.Write("\nEnter Your Id:");
                if (int.TryParse(Console.ReadLine(), out id))
                    break;
                Console.WriteLine("Invalid input! Please enter a number.");
            }
            var passenger = new Passenger { Id = id };

            int flightNmuber;
            Flight? flight = null;
            while (flight == null)
            {
                Console.Write("Enter Flight Nmumber you want to book:");
                if (int.TryParse(Console.ReadLine(), out flightNmuber))
                {
                    flight = allFlights.FirstOrDefault(f => f.FlightNumber == flightNmuber);
                    if (flight == null)
                    {
                        Console.WriteLine("Flight not found.");
                        continue;
                    }
                }
                if (flight != null)
                    break;
                Console.WriteLine("Invalid input! Please enter a number.");
            }

            FlightClassType flightClass;
            while (true)
            {
                Console.Write("Enter Flight Class (Economy, Business, First_Class): ");
                var input = Console.ReadLine();
                if (Enum.TryParse(input, true, out flightClass))
                {
                    break;
                }
                Console.WriteLine("Invalid flight class.");
            }

            passengerService.BookFlight(passenger, flight, flightClass);
            Console.WriteLine("Flight booked successfully");

        }
    }

    public static List<Booking> ViewPersonalBookings(PassengerService passengerService)
    {
        int id;
        while (true)
        {
            Console.Write("\nEnter Your Id:");
            if (int.TryParse(Console.ReadLine(), out id))
                break;
            Console.WriteLine("Invalid input! Please enter a number.");
        }

        var personalBookings = passengerService.ViewPersonalBookings(id);
        if (personalBookings.Any())
            Display(personalBookings);
        else
            Console.WriteLine("No Bookings found.");

        return personalBookings;

    }

    public static void ModifyBooking(PassengerService passengerService)
    {
        var personalBookings = ViewPersonalBookings(passengerService);

        int bookingId;

        Booking? modifiedBooking = null;
        while (modifiedBooking == null)
        {
            Console.Write("\nEnter Booking Id to modify:");
            if (int.TryParse(Console.ReadLine(), out bookingId))
            {
                modifiedBooking = personalBookings.FirstOrDefault(b => b.BookingId == bookingId);
                if (modifiedBooking == null)
                {
                    Console.WriteLine("Booking not found! Please enter a valid Booking Id.");
                    continue;
                }
            }
            if (modifiedBooking != null)
                break;
            Console.WriteLine("Invalid input! Please enter a number.");
        }


        FlightClassType newflightClass;
        while (true)
        {
            Console.Write("Enter new Flight Class (Economy, Business, First_Class): ");
            var input = Console.ReadLine();
            if (Enum.TryParse(input, true, out newflightClass))
            {
                break;
            }
            Console.WriteLine("Invalid flight class.");
        }

        modifiedBooking.Class = newflightClass;

        if (passengerService.ModifyBooking(modifiedBooking))
            Console.WriteLine("Booking modified successfully.");


    }

    public static void CancelBooking(PassengerService passengerService)
    {
        var personalBookings = ViewPersonalBookings(passengerService);

        int bookingId = 0;

        Booking? canceledBooking = null;
        while (canceledBooking == null)
        {
            Console.Write("\nEnter Booking Id to cancel:");
            if (int.TryParse(Console.ReadLine(), out bookingId))
            {
                canceledBooking = personalBookings.FirstOrDefault(b => b.BookingId == bookingId);
                if (canceledBooking == null)
                {
                    Console.WriteLine("Booking not found! Please enter a valid Booking Id.");
                    continue;
                }
            }
            if (canceledBooking != null)
                break;
            Console.WriteLine("Invalid input! Please enter a number.");
        }

        if (passengerService.CancelBooking(bookingId))
            Console.WriteLine("Booking caneled successfully.");
    }

    public static void ViewAllAvailableFlights(FlightMap flightMap)
    {
        var allFlights = flightMap.GetAllFlights();
        if (allFlights.Any())
        {
            Display(allFlights);
        }
        else
            Console.WriteLine("No flights available.");
    }

}