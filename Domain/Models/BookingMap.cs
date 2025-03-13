using Airport_Ticket_Booking.Domain.FlightManagement;

namespace Airport_Ticket_Booking.Domain.Models;

public class BookingMap
{
    private FileHandler _fileHandler;
    private static readonly string _filePath = "C:\\Users\\hp\\source\\repos\\Airport Ticket Booking\\Data\\bookings.csv";

    public BookingMap(FileHandler fileHandler)
    {
        _fileHandler = fileHandler;
    }

    public List<Booking> GetAllBookings()
    {
        return _fileHandler.ReadFromFile<Booking>(_filePath);
    }

    public void SaveBookings(List<Booking> allBookings)
    {
        _fileHandler.WriteToFile<Booking>(allBookings, _filePath);

    }
}