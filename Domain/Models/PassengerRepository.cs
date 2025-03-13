using Airport_Ticket_Booking.Infrastructure.Storage;

namespace Airport_Ticket_Booking.Domain.Models;

public class PassengerRepository
{
    private FileHandler _fileHandler;
    private static readonly string _filePath = "C:\\Users\\hp\\source\\repos\\Airport Ticket Booking\\Infrastructure\\Data\\passengers.csv";

    public PassengerRepository(FileHandler fileHandler)
    {
        _fileHandler = fileHandler;
    }

    public List<Passenger> GetAllPassengers()
    {
        return _fileHandler.ReadFromFile<Passenger>(_filePath);
    }

    public void SavePassengerInfo(List<Passenger> allPassengers)
    {
        _fileHandler.WriteToFile<Passenger>(allPassengers, _filePath);

    }
}