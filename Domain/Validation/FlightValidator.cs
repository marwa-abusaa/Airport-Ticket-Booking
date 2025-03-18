using Airport_Ticket_Booking.Domain.Models;
using Airport_Ticket_Booking.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Airport_Ticket_Booking.Domain.Validation;

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
   
}