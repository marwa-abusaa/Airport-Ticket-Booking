
using Airport_Ticket_Booking.Domain.General;
using Airport_Ticket_Booking.Domain.Records;
using Airport_Ticket_Booking.Services;
using System.Globalization;

public class Program
{
    private static readonly string _filePath = "C:\\Users\\hp\\source\\repos\\Airport Ticket Booking\\Data\\flights.csv";

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
            Console.WriteLine("\nFlightNumber DepartureCountry DestinationCountry DepartureDate DepartureAirport ArrivalAirport Class Price\n");
            foreach(var f in flights)
            {
                Console.WriteLine(f);
            }
        }
        else
            Console.WriteLine("No flights found.");
    }
    public static void BookFlight()
    {

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
        FileHandler fileHandler = new FileHandler(_filePath);
        BookingService bookingService = new BookingService(fileHandler);
        PassengerService passengerService = new PassengerService(bookingService, fileHandler);
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
                    BookFlight();
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
    