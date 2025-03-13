using Airport_Ticket_Booking.Infrastructure.Storage;

namespace Airport_Ticket_Booking.Domain.Models;

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
        _fileHandler.WriteToFile<Flight>(allFlights, _filePath);

    }
}