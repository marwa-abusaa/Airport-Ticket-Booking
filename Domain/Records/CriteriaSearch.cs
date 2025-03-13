
namespace Airport_Ticket_Booking.Domain.Records;

public record class CriteriaSearch(        
    string? departureCountry = null, 
    string? destinationCountry = null,
    DateTime? departureDate = null,   
    string? departureAirport = null,
    string? arrivalAirport = null,
    string? flightClass = null,
    decimal? maxPrice = null
);