using Airport_Ticket_Booking.Domain.General;
using Airport_Ticket_Booking.Validation;
using System.ComponentModel.DataAnnotations;

namespace Airport_Ticket_Booking.Domain.FlightManagement
{
    public class Flight
    {
        [Required(ErrorMessage = "Flight Number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Flight Number must be a positive integer")]
        public int FlightNumber { get; set; }


        [Required(ErrorMessage = "Departure Country is required.")]
        public string DepartureCountry { get; set; }


        [Required(ErrorMessage = "Destination Country is required.")]
        public string DestinationCountry { get; set; }


        [Required(ErrorMessage = "Departure Date is required.")]
        [FutureDate(ErrorMessage = "Departure Date must be in the future.")]
        public DateTime DepartureDate { get; set; }


        [Required(ErrorMessage = "Departure Airport is required.")]
        public string DepartureAirport { get; set; }


        [Required(ErrorMessage = "Arrival Airport is required.")]
        public string ArrivalAirport { get; set; }


        [Required(ErrorMessage = "Class is required.")]
        public FlightClassType Class { get; set; }


        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{FlightNumber},{DepartureCountry},{DestinationCountry},{DepartureDate},{DepartureAirport},{ArrivalAirport},{Class},{Price}";
        }
    }
}
