using Airport_Ticket_Booking.Domain.General;

namespace Airport_Ticket_Booking.Domain.FlightManagement
{
    public class Flight
    {
        public int FlightNumber { get; set; }       
        public string DepartureCountry { get; set; }
        public string DestinationCountry { get; set; }
        public DateTime DepartureDate { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public FlightClassType Class { get; set; }
        public decimal Price { get; set; }
        public override string ToString()
        {
            return $"{FlightNumber},{DepartureCountry},{DestinationCountry},{DepartureDate},{DepartureAirport},{ArrivalAirport},{Class},{Price}";
        }
    }
}
