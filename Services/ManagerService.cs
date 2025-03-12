
using Airport_Ticket_Booking.Domain.FlightManagement;
using Airport_Ticket_Booking.Domain.Models;
using Airport_Ticket_Booking.Domain.Records;
using System.ComponentModel.DataAnnotations;

namespace Airport_Ticket_Booking.Services
{
    public class ManagerService
    {
        private FlightMap _flightMap;
        private BookingMap _bookingMap;

        public ManagerService(BookingMap bookingMap, FlightMap flightMap)
        {
            _flightMap = flightMap;
            _bookingMap = bookingMap;
        }

        public List<Booking> FilterBookings(CriteriaFilter criteria)
        {
            var allFlights = _flightMap.GetAllFlights();
            var allBookins = _bookingMap.GetAllBookings();
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

        public List<ValidationResult> ImportFlightsFromCsv(string filePath)
        {
            var fileHandler = new FileHandler();
            return ValidateImportedFlightData(fileHandler, filePath);


        }

        public List<ValidationResult> ValidateImportedFlightData(FileHandler fileHandler, string filePath)
        {
            var allFlights = fileHandler.ReadFromFile<Flight>(filePath);
            var validationResults = new List<ValidationResult>();

            foreach (var flight in allFlights)
            {
                var context = new ValidationContext(flight, null, null);
                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(flight, context, results, true);

                if (!isValid)
                {
                    validationResults.AddRange(results);
                }
            }
            return validationResults;
        }
        
    }
}
