namespace Airport_Ticket_Booking.Domain.Models;

public record class CriteriaFilter(
        int? flightId = null,
        int? passengerId = null,
        string? departureCountry = null,
        string? destinationCountry = null,
        DateTime? departureDate = null,
        string? departureAirport = null,
        string? arrivalAirport = null,
        string? flightClass = null,
        decimal? maxPrice = null
    );