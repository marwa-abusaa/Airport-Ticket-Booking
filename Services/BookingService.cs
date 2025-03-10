
using Airport_Ticket_Booking.Domain.FlightManagement;

namespace Airport_Ticket_Booking.Services
{
   public class BookingService
    {
        private readonly string _filePath = "C:\\Users\\hp\\source\\repos\\Airport Ticket Booking\\Data\\flights.csv";

        private FileHandler _fileHandler;

        public BookingService(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }
        public List<Booking> GetAllBookings()
        {
            return _fileHandler.ReadFromFile<Booking>();
        }
        public void BookFlight(Booking booking)
        {
            var allBookings = GetAllBookings();
            if (allBookings.Any())
            {
                allBookings.Add(booking);
                _fileHandler.WriteToFile<Booking>(allBookings);
            }

        }
        public bool CancelBooking(int bookingId)
        {
            var allBookings = GetAllBookings();
            var booking = allBookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking != null)
            {
                allBookings.Remove(booking);
                _fileHandler.WriteToFile<Booking>(allBookings);
                return true;
            }
            return false;
        }
        public bool ModifyBooking(Booking updateBooking)
        {
            var allBookings = GetAllBookings();
            var booking = allBookings.FirstOrDefault(b => b.BookingId == updateBooking.BookingId);
            if (booking != null)
            {
                allBookings.Remove(booking);
                allBookings.Add(updateBooking);
                _fileHandler.WriteToFile<Booking>(allBookings);
                return true;
            }
            return false;
        }
        public List<Booking> GetBookingsForPassenger(int passengerId)
        {
            var allBookings = GetAllBookings();
            var passengerBookings = allBookings.TakeWhile(p => p.PassengerId == passengerId).ToList();
            return passengerBookings;
        }

    }
}
