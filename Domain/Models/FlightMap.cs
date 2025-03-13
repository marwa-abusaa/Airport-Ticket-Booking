using Airport_Ticket_Booking.Domain.FlightManagement;
using Airport_Ticket_Booking.Storage;

namespace Airport_Ticket_Booking.Domain.Models;

public class FlightMap
{
    private FileHandler _fileHandler;
    private static readonly string _filePath = "C:\\Users\\hp\\source\\repos\\Airport Ticket Booking\\Data\\flights.csv";

    public FlightMap(FileHandler fileHandler)
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