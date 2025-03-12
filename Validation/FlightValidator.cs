using Airport_Ticket_Booking.Domain.FlightManagement;
using Airport_Ticket_Booking.Domain.General;
using System.ComponentModel.DataAnnotations;
using System.Globalization;


namespace Airport_Ticket_Booking.Validation
{
  
    public class FlightValidator
    {
        public Dictionary<string, string> GetValidationConstraints()
        {
            var constraints = new Dictionary<string, string>();

            var properties = typeof(Flight).GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(true);
                var constraintDescription = new List<string>();

                foreach (var attribute in attributes)
                {
                    if (attribute is RequiredAttribute)
                    {
                        constraintDescription.Add("Required");
                    }
                    else if (attribute is RangeAttribute range)
                    {
                        constraintDescription.Add($"Allowed Range: {range.Minimum} → {range.Maximum}");
                    }
                    else if (attribute is FutureDateAttribute)
                    {
                        constraintDescription.Add("Must be a future date");
                    }
                }

                if (constraintDescription.Any())
                {
                    constraints[property.Name] = string.Join(", ", constraintDescription);
                }
            }

            return constraints;
        }
        public List<ValidationResult> ValidateFlight(Flight flight)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(flight);
            Validator.TryValidateObject(flight, context, results, validateAllProperties: true);
            return results;
        }
        public Flight ParesdFlight(string line, List<ValidationResult> validationResults)
        {
            var parts = line.Split(',');

            var flight = new Flight();
            if (parts.Length != 8)
            {
                validationResults.Add(new ValidationResult($"Invalid number of columns in line: {line}"));
            }

            if (!int.TryParse(parts[0], out int flightNumber))
            {
                validationResults.Add(new ValidationResult("Flight Number must be a valid integer."));
            }
            else
            {
                flight.FlightNumber = flightNumber;
            }

            flight.DepartureCountry = parts[1];
            flight.DestinationCountry = parts[2];

            if (!DateTime.TryParseExact(parts[3], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureDate))
            {
                validationResults.Add(new ValidationResult("Departure Date must be in the correct format (yyyy-MM-dd HH:mm:ss)."));
            }
            else if (departureDate <= DateTime.Now) 
            {
                validationResults.Add(new ValidationResult("Departure Date must be in the future."));
            }
            else
            {
                flight.DepartureDate = departureDate;
            }

            flight.DepartureAirport = parts[4];
            flight.ArrivalAirport = parts[5];

            if (!Enum.TryParse(parts[6], out FlightClassType flightClass))
            {
                validationResults.Add(new ValidationResult("Class must be a valid FlightClassType (Economy, Business, First_Class)."));
            }
            else
            {
                flight.Class = flightClass;
            }

            if (!decimal.TryParse(parts[7], CultureInfo.InvariantCulture, out decimal price))
            {
                validationResults.Add(new ValidationResult("Price must be a valid decimal number."));
            }
            else if (price < 0)
            {
                validationResults.Add(new ValidationResult("Price must be a positive number."));
            }
            else
            {
                flight.Price = price;
            }

            return flight;
        }
       
    }
}
