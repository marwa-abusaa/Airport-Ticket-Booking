
using Airport_Ticket_Booking.Domain.Models;
using Airport_Ticket_Booking.Domain.Records;
using Airport_Ticket_Booking.Infrastructure.Repositories;
using Airport_Ticket_Booking.Validation;
using System.ComponentModel.DataAnnotations;

namespace Airport_Ticket_Booking.Services;

public class ManagerService
{
    private FlightRepository _flightMap;
    private BookingRepository _bookingMap;
    private FlightValidator validator = new FlightValidator();

    public ManagerService(BookingRepository bookingMap, FlightRepository flightMap)
    {
        _flightMap = flightMap;
        _bookingMap = bookingMap;
    }

    public List<Booking> FilterBookings(CriteriaFilter filter)
    {
        var allFlights = _flightMap.GetAllFlights();
        var allBookins = _bookingMap.GetAllBookings();

        var filteredBookings = allBookins
            .Join(allFlights,
                booking => booking.FlightId,
                flight => flight.FlightNumber,
                (booking, flight) => new { Booking = booking, Flight = flight }
            )
            .Where(x =>
                (!filter.flightId.HasValue || x.Flight.FlightNumber == filter.flightId) &&
                (!filter.passengerId.HasValue || x.Booking.PassengerId == filter.passengerId ) &&
                (string.IsNullOrEmpty(filter.departureCountry) || x.Flight.DepartureCountry.Equals(filter.departureCountry, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(filter.destinationCountry) || x.Flight.DestinationCountry.Equals(filter.destinationCountry, StringComparison.OrdinalIgnoreCase)) &&
                (!filter.departureDate.HasValue || x.Flight.DepartureDate.Date == filter.departureDate.Value.Date) &&
                (string.IsNullOrEmpty(filter.departureAirport) || x.Flight.DepartureAirport.Equals(filter.departureAirport, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(filter.arrivalAirport) || x.Flight.ArrivalAirport.Equals(filter.arrivalAirport, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(filter.flightClass) || x.Flight.Class.ToString().Equals(filter.flightClass, StringComparison.OrdinalIgnoreCase)) &&
                (!filter.maxPrice.HasValue || x.Flight.Price <= filter.maxPrice)
            )
            .Select(x => x.Booking);

        return filteredBookings.ToList();
    }

    public List<ValidationResult> ImportFlightsFromCsv(string filePath)
    {
        var validationResults = new List<ValidationResult>();
        var lines = File.ReadAllLines(filePath).Skip(1); 

        foreach (var line in lines)
        {
            var flight = validator.ParesdFlight(line, validationResults); 
            var results = validator.ValidateFlight(flight); 

            if (results.Any())
            {
                validationResults.AddRange(results); 
            }
        }

        return validationResults; 
    }

}
