using Airport_Ticket_Booking.Domain.Enums;
using Airport_Ticket_Booking.Domain.Models;
using Airport_Ticket_Booking.Infrastructure.Storage;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Airport_Ticket_Booking.Infrastructure.Repositories;

public class FlightRepository
{
    private FileHandler _fileHandler;
    private static readonly string _filePath = "C:\\Users\\hp\\source\\repos\\Airport Ticket Booking\\Infrastructure\\Data\\flights.csv";

    public FlightRepository(FileHandler fileHandler)
    {
        _fileHandler = fileHandler;
    }

    public List<Flight> GetAllFlights()
    {
        return _fileHandler.ReadFromFile<Flight>(_filePath);
    }

    public void SaveFlights(List<Flight> allFlights)
    {
        _fileHandler.WriteToFile(allFlights, _filePath);

    }

    public Flight ParesdFlight(string line, List<ValidationResult> validationResults)
    {
        var parts = line.Split(',');

        var flight = new Flight();
        if (parts.Length != 8)
        {
            validationResults.Add(new ValidationResult($"Invalid number of columns in line: {line}"));
        }

        if (!int.TryParse(parts[0], out int flightNumber))
        {
            validationResults.Add(new ValidationResult("Flight Number must be a valid integer."));
        }
        else
        {
            flight.FlightNumber = flightNumber;
        }

        flight.DepartureCountry = parts[1];
        flight.DestinationCountry = parts[2];

        if (!DateTime.TryParseExact(parts[3], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureDate))
        {
            validationResults.Add(new ValidationResult("Departure Date must be in the correct format (yyyy-MM-dd HH:mm:ss)."));
        }
        else if (departureDate <= DateTime.Now)
        {
            validationResults.Add(new ValidationResult("Departure Date must be in the future."));
        }
        else
        {
            flight.DepartureDate = departureDate;
        }

        flight.DepartureAirport = parts[4];
        flight.ArrivalAirport = parts[5];

        if (!Enum.TryParse(parts[6], out FlightClassType flightClass))
        {
            validationResults.Add(new ValidationResult("Class must be a valid FlightClassType (Economy, Business, First_Class)."));
        }
        else
        {
            flight.Class = flightClass;
        }

        if (!decimal.TryParse(parts[7], CultureInfo.InvariantCulture, out decimal price))
        {
            validationResults.Add(new ValidationResult("Price must be a valid decimal number."));
        }
        else if (price < 0)
        {
            validationResults.Add(new ValidationResult("Price must be a positive number."));
        }
        else
        {
            flight.Price = price;
        }

        return flight;
    }
}