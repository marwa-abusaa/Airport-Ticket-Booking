using Airport_Ticket_Booking.Domain.Models;

namespace Airport_Ticket_Booking.Services;

public class BookingService
{

    private BookingRepository _bookingMap;
    private PassengerRepository _passengerRepository;


    public BookingService(BookingRepository bookingMap, PassengerRepository passengerRepository)
    {
        _bookingMap = bookingMap;
        _passengerRepository = passengerRepository;
    }

    public void BookFlight(Booking booking)
    {
        var allBookings = _bookingMap.GetAllBookings();
        if (allBookings.Any())
        {
            allBookings.Add(booking);
            _bookingMap.SaveBookings(allBookings);
        }

    }

    public bool CancelBooking(int bookingId)
    {
        var allBookings = _bookingMap.GetAllBookings();
        var booking = allBookings.FirstOrDefault(b => b.BookingId == bookingId);
        if (booking != null)
        {
            allBookings.Remove(booking);
            _bookingMap.SaveBookings(allBookings);
            return true;
        }
        return false;
    }

    public bool ModifyBooking(Booking updateBooking)
    {
        var allBookings = _bookingMap.GetAllBookings();
        var booking = allBookings.FirstOrDefault(b => b.BookingId == updateBooking.BookingId);
        if (booking != null)
        {
            allBookings.Remove(booking);
            allBookings.Add(updateBooking);
            _bookingMap.SaveBookings(allBookings);
            return true;
        }
        return false;
    }

    public List<Booking> GetBookingsForPassenger(int passengerId)
    {
        var allBookings = _bookingMap.GetAllBookings();
        var passengerBookings = allBookings.Where(p => p.PassengerId == passengerId).ToList();
        return passengerBookings;
    }

    public bool AddPassenger(Passenger passenger)
    {
        var passengers = _passengerRepository.GetAllPassengers();
        bool exists = passengers.Any(p => p.Id == passenger.Id);
        if (!exists)
        {
            passengers.Add(passenger);
            _passengerRepository.SavePassengerInfo(passengers);
            return true;
        }
        return false;
    }

}