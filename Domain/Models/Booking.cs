
using Airport_Ticket_Booking.Domain.General;

namespace Airport_Ticket_Booking.Domain.FlightManagement
{
    public class Booking
    {
        public int BookingId { get; set; }       
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public FlightClassType Class { get; set; }
        public decimal Price { get; set; }
        public override string ToString()
        {
            return $"{BookingId} {FlightId} {Class} {Price}";
        }
    }
}
