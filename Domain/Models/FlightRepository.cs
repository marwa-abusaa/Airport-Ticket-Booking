using Airport_Ticket_Booking.Infrastructure.Storage;

namespace Airport_Ticket_Booking.Domain.Models;

public class FlightRepository
{
    private FileHandler _fileHandler;
    private static readonly string _filePath = "C:\\Users\\hp\\source\\repos\\Airport Ticket Booking\\Data\\flights.csv";

    public FlightRepository(FileHandler fileHandler)
    {
        _fileHandler = fileHandler;
    }

    public List<FlightRepository> GetAllFlights()
    {
        return _fileHandler.ReadFromFile<FlightRepository>(_filePath);
    }

    public void SaveFlights(List<FlightRepository> allFlights)
    {
        _fileHandler.WriteToFile<FlightRepository>(allFlights, _filePath);

    }
}