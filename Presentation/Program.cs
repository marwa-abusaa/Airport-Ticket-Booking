
using Airport_Ticket_Booking.Domain.FlightManagement;
using Airport_Ticket_Booking.Domain.General;
using Airport_Ticket_Booking.Domain.Models;
using Airport_Ticket_Booking.Domain.Records;
using Airport_Ticket_Booking.Services;
using System.Globalization;

public class Program
{

    public static void DisplayFlights(List<Flight> flights)
    {
        Console.WriteLine("\nFlightNumber DepartureCountry DestinationCountry DepartureDate DepartureAirport ArrivalAirport Class Price\n");
        foreach (var f in flights)
        {
            Console.WriteLine(f);
        }
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

        decimal? maxPrice=null;
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

        criteria = new CriteriaSearch (
            departureCountry,
            destinationCountry,
            departureDate,
            departureAirport,
            arrivalAirport,
            Class,
            maxPrice
            );

        var flights= passengerService.SearchAvailableFlights(criteria);
        if (flights != null)
        {
            DisplayFlights(flights);
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
            DisplayFlights(allFlights);
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
            Flight? flight=null;
            while (flight==null)
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
    public static void ViewPersonalBookings()
    {

    }
    public static void ModifyBooking()
    {

    }
    public static void CancelBooking()
    {

    }
    public static void ViewAllAvailableFlights()
    {

    }
    public static void FilterBookings()
    {

    }
    public static void ImportFlightsFromCSV()
    {

    }
    public static void Menu()
    {        
        Console.WriteLine("\n1. Search Available Flights");
        Console.WriteLine("2. Book a Flight");
        Console.WriteLine("3. View Personal Bookings");
        Console.WriteLine("4. Modify a Booking");
        Console.WriteLine("5. Cancel a Booking");        
        Console.WriteLine("6. View All Available Flights");
        Console.WriteLine("7. Filter Bookings");
        Console.WriteLine("8. Import Flights from CSV");
        Console.WriteLine("9. Exit");
        Console.Write("\nSelect an option: ");        
    }

    private static void Main(string[] args)
    {
        FileHandler fileHandler = new FileHandler();
        BookingMap bookingMap = new BookingMap(fileHandler);
        FlightMap flightMap = new FlightMap(fileHandler);
        BookingService bookingService = new BookingService(bookingMap);
        PassengerService passengerService = new PassengerService(bookingService, flightMap);
        CriteriaSearch criteriaSearch = new CriteriaSearch();

        Console.WriteLine("Airport Ticket Booking System");

        while (true)
        {
            Menu();

            switch (Console.ReadLine())
            {
                case "1":
                    SearchAvailableFlights(criteriaSearch, passengerService);
                    break;
                case "2":
                    BookFlight(passengerService, flightMap);
                    break;
                case "3":
                    ViewPersonalBookings();
                    break;
                case "4":
                    ModifyBooking();
                    break;
                case "5":
                    CancelBooking();
                    break;
                case "6":
                    ViewAllAvailableFlights();
                    break;
                case "7":
                    FilterBookings();
                    break;
                case "8":
                    ImportFlightsFromCSV();
                    break;
                case "9":
                    return;
                default:
                    {
                        Console.WriteLine("Please, choose a valid number!\n");
                        break;
                    }

            }
        }
    }
        
}
    