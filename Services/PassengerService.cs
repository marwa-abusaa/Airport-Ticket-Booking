
using Airport_Ticket_Booking.Domain.FlightManagement;
using Airport_Ticket_Booking.Domain.General;

namespace Airport_Ticket_Booking.Services
{
    public class PassengerService
    {
        private readonly BookingService _bookingService;

        public PassengerService(BookingService bookingService)
        {
            _bookingService = bookingService;
        }
        public void BookFlight(Passenger passenger, Flight flight)
        {
            var booking = new Booking
            {
                BookingId = GenerateBookingId(),
                FlightId = flight.FlightNumber,
                PassengerId = passenger.Id,
                Class = flight.Class,
                Price = flight.Price
            };
            _bookingService.BookFlight(booking);
        }

        public void CancelBooking(int bookingId)
        {
            _bookingService.CancelBooking(bookingId);
        }

        public void ModifyBooking(Booking updateBooking)
        {
            _bookingService.ModifyBooking(updateBooking);
        }
        
        private int GenerateBookingId()
        {
            return new Random().Next(1, 99999);
        }
    }
}
}
