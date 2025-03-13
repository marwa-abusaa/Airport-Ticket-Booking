using Airport_Ticket_Booking.Domain.Models;
using Airport_Ticket_Booking.Infrastructure.Storage;

namespace Airport_Ticket_Booking.Infrastructure.Repositories;

public class BookingRepository
{
    private FileHandler _fileHandler;
    private static readonly string _filePath = "C:\\Users\\hp\\source\\repos\\Airport Ticket Booking\\Infrastructure\\Data\\bookings.csv";

    public BookingRepository(FileHandler fileHandler)
    {
        _fileHandler = fileHandler;
    }

    public List<Booking> GetAllBookings()
    {
        return _fileHandler.ReadFromFile<Booking>(_filePath);
    }

    public void SaveBookings(List<Booking> allBookings)
    {
        _fileHandler.WriteToFile(allBookings, _filePath);

    }
}