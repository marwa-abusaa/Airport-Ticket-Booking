using Airport_Ticket_Booking.Domain.FlightManagement;
using Airport_Ticket_Booking.Domain.General;
using Airport_Ticket_Booking.Domain.Models;
using Airport_Ticket_Booking.Domain.Records;
using Airport_Ticket_Booking.Services;
using Airport_Ticket_Booking.Validation;
using System.Globalization;


namespace Airport_Ticket_Booking.Presentation.Menu;

public class ManagerMenu
{
    public static void ManagersMenu()
    {
        Console.WriteLine("\n1. Filter Bookings");
        Console.WriteLine("2. Import Flights from CSV");
        Console.WriteLine("3. Get Flight validation");
        Console.WriteLine("4. Exit");
        Console.Write("\nSelect an option: ");
    }

    public static void ManagerActions()
    {
        FileHandler fileHandler = new FileHandler();
        BookingMap bookingMap = new BookingMap(fileHandler);
        FlightMap flightMap = new FlightMap(fileHandler);
        CriteriaFilter criteriaFilter = new CriteriaFilter();
        ManagerService managerService = new ManagerService(bookingMap, flightMap);
        FlightValidator flightValidator = new FlightValidator();

        Console.WriteLine("Welcome Manager! What would you like to do?");
        while (true)
        {
            ManagersMenu();

            switch (Console.ReadLine())
            {
                case "1":
                    FilterBookings(managerService, criteriaFilter, flightMap);
                    break;
                case "2":
                    ImportFlightsFromCSV(managerService, flightValidator);
                    break;
                case "3":
                    GetFlightvalidation(flightValidator);
                    break;
                case "4":
                    return;
                default:
                    {
                        Console.WriteLine("Please, choose a valid number!\n");
                        break;
                    }


            }
        }
    }

    public static void DisplayBookings(List<Booking> bookings)
    {
        Console.WriteLine("\nBookingId FlightId Class Price\n");
        foreach (var b in bookings)
            Console.WriteLine(b);
    }
    public static void FilterBookings(ManagerService managerService, CriteriaFilter criteria, FlightMap flightMap)
    {
        Console.Write("For each field, enter a value or press ENTER to skip and leave it empty.\n");

        int? flightId = null;
        while (true)
        {
            Console.Write("Enter Flight Number: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                flightId = null;
                break;
            }

            else if (int.TryParse(input, out int parsedFlightId))
            {
                flightId = parsedFlightId;
                break;
            }
            Console.WriteLine("Invalid input! Please enter a number.");
        }

        int? passengerId = null;
        while (true)
        {
            Console.Write("Enter Passenger Id: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                passengerId = null;
                break;
            }

            else if (int.TryParse(input, out int parsedPassengerId))
            {
                passengerId = parsedPassengerId;
                break;
            }
            Console.WriteLine("Invalid input! Please enter a number.");
        }

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

        criteria = new CriteriaFilter(
            flightId,
            passengerId,
            departureCountry,
            destinationCountry,
            departureDate,
            departureAirport,
            arrivalAirport,
            Class,
            maxPrice
            );

        var bookings = managerService.FilterBookings(criteria);
        if (bookings.Any())
        {
            DisplayBookings(bookings);
        }
        else
            Console.WriteLine("No bookings found.");
    }

    public static void ImportFlightsFromCSV(ManagerService managerService, FlightValidator flightValidator)
    {
        Console.Write("Enter CSV file path for flights import: ");
        var filePath = Console.ReadLine();
        var validationResults = managerService.ImportFlightsFromCsv(filePath);

        Console.WriteLine();

        if (validationResults.Any())
        {
            foreach (var error in validationResults)
            {
                Console.WriteLine(error.ErrorMessage);
            }
        }
        else
            Console.WriteLine("Flights imported successfully.");

        Console.WriteLine();

    }

    public static void GetFlightvalidation(FlightValidator flightValidator)
    {

        var constraints = flightValidator.GetValidationConstraints();

        foreach (var constraint in constraints)
        {
            Console.WriteLine($"{constraint.Key}: {constraint.Value}");
        }
    }
}
