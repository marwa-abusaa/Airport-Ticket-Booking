using Airport_Ticket_Booking.Domain.FlightManagement;
using Airport_Ticket_Booking.Domain.General;
using Airport_Ticket_Booking.Domain.Records;

namespace Airport_Ticket_Booking.Services
{
    public class PassengerService
    {
        private readonly BookingService _bookingService;
        private FileHandler _fileHandler;

        public PassengerService(BookingService bookingService, FileHandler fileHandler)
        {
            _bookingService = bookingService;
            _fileHandler = fileHandler;
        }

        public void BookFlight(Passenger passenger, Flight flight)
        {
            var booking = new Booking
            {
                BookingId = GenerateBookingId(),
                FlightId = flight.FlightNumber,
                PassengerId = passenger.Id,
                Class = flight.Class,
                Price = CalculatePrice(flight.Price, flight.Class)
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

        public List<Flight> SearchAvailableFlights(CriteriaSearch criteria)
        {
            var allFlights= _fileHandler.ReadFromFile<Flight>();
            var availableFlights = allFlights.Where(f =>
                (string.IsNullOrEmpty(criteria.departureCountry) || f.DepartureCountry == criteria.departureCountry) &&
                (string.IsNullOrEmpty(criteria.destinationCountry) || f.DestinationCountry == criteria.destinationCountry) &&
                (!criteria.departureDate.HasValue || f.DepartureDate.Date.Equals(criteria.departureDate.Value)) &&
                (string.IsNullOrEmpty(criteria.departureAirport) || f.DepartureAirport == criteria.departureAirport) &&
                (string.IsNullOrEmpty(criteria.arrivalAirport) || f.ArrivalAirport == criteria.arrivalAirport) &&
                (string.IsNullOrEmpty(criteria.flightClass) || f.Class.ToString().Equals(criteria.flightClass, StringComparison.OrdinalIgnoreCase) ) &&
                (!criteria.maxPrice.HasValue || f.Price <= criteria.maxPrice.Value)
            ).ToList();

            return availableFlights;
        }

        public List<Booking> ViewPersonalBookings(int passengerId)
        {
            var PersonalBookings = _bookingService.GetBookingsForPassenger(passengerId);
            return PersonalBookings;
        }

        public decimal CalculatePrice(decimal originPrice, FlightClassType flightClass)
        {
            switch (flightClass)
            {
                case FlightClassType.Economy:
                    return originPrice;
                case FlightClassType.Business:
                    return originPrice * 2.0m;
                case FlightClassType.First_Class:
                    return originPrice * 3.0m;
            }
            return 0;
        }

        private int GenerateBookingId()
        {
            return new Random().Next(1, 99999);
        }
    }
}

