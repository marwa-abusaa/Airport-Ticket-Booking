using Airport_Ticket_Booking.Domain.FlightManagement;
using Airport_Ticket_Booking.Domain.General;
using Airport_Ticket_Booking.Domain.Models;
using Airport_Ticket_Booking.Domain.Records;

namespace Airport_Ticket_Booking.Services
{
    public class PassengerService
    {
        private readonly BookingService _bookingService;
        private FlightMap _flightMap;


        public PassengerService(BookingService bookingService, FlightMap flightMap)
        {
            _bookingService = bookingService;
            _flightMap = flightMap;
        }

        public void BookFlight(Passenger passenger, Flight flight, FlightClassType classType)
        {
            var booking = new Booking
            {
                BookingId = GenerateBookingId(),
                FlightId = flight.FlightNumber,
                PassengerId = passenger.Id,
                Class = classType,
                Price = CalculatePrice(flight.Price, flight.Class)
            };
            _bookingService.BookFlight(booking);
        }

        public bool CancelBooking(int bookingId)
        {
            return _bookingService.CancelBooking(bookingId);
        }

        public bool ModifyBooking(Booking updateBooking)
        {
            return _bookingService.ModifyBooking(updateBooking);
        }

        public List<Flight> SearchAvailableFlights(CriteriaSearch criteria)
        {
            var allFlights = _flightMap.GetAllFlights();
            var availableFlights = allFlights.Where(f =>
                (string.IsNullOrEmpty(criteria.departureCountry) || f.DepartureCountry.Equals(criteria.departureCountry, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(criteria.destinationCountry) || f.DestinationCountry.Equals(criteria.destinationCountry, StringComparison.OrdinalIgnoreCase) ) &&
                (!criteria.departureDate.HasValue || f.DepartureDate.Date.Equals(criteria.departureDate.Value)) &&
                (string.IsNullOrEmpty(criteria.departureAirport) || f.DepartureAirport.Equals(criteria.departureAirport, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(criteria.arrivalAirport) || f.ArrivalAirport.Equals(criteria.arrivalAirport, StringComparison.OrdinalIgnoreCase)) &&
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

