
using Airport_Ticket_Booking.Domain.FlightManagement;
using Airport_Ticket_Booking.Domain.Records;

namespace Airport_Ticket_Booking.Services
{
    public class ManagerService
    {
        private FileHandler _fileHandler;

        public ManagerService(BookingService bookingService, FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }
        public List<Booking> FilterBookings(CriteriaFilter criteria)
        {
            var allFlights = _fileHandler.ReadFromFile<Flight>();
            var allBookins = _fileHandler.ReadFromFile<Booking>();
            var filterdBookins = allBookins.Join(allFlights,
                booking => booking.FlightId,
                flight => flight.FlightNumber,               
                (booking, flight) => new { booking, flight })               
                .Where(f =>
                (!criteria.flightNumber.HasValue || f.flight.FlightNumber == criteria.flightNumber.Value) &&
                (string.IsNullOrEmpty(criteria.departureCountry) || f.flight.DepartureCountry == criteria.departureCountry) &&
                (string.IsNullOrEmpty(criteria.destinationCountry) || f.flight.DestinationCountry == criteria.destinationCountry) &&
                (!criteria.departureDate.HasValue || f.flight.DepartureDate == criteria.departureDate.Value) &&
                (string.IsNullOrEmpty(criteria.departureAirport) || f.flight.DepartureAirport == criteria.departureAirport) &&
                (string.IsNullOrEmpty(criteria.arrivalAirport) || f.flight.ArrivalAirport == criteria.arrivalAirport) &&
                (string.IsNullOrEmpty(criteria.flightClass) || f.flight.Class.ToString().Equals(criteria.flightClass, StringComparison.OrdinalIgnoreCase)) &&
                (!criteria.maxPrice.HasValue || f.flight.Price <= criteria.maxPrice.Value)
                )                    
                .Select(b => b.booking)
                .ToList();

            return filterdBookins;
        }
      
        }
    }
}
