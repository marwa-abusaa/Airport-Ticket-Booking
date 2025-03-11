using Airport_Ticket_Booking.Domain.FlightManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Validation
{
    public class ValidationDetails
    {
        public string PropertyName { get; set; }
        public string Type { get; set; }
        public List<string> Constraints { get; set; } = new List<string>();
    }

    public class FlightValidator
    {
        public static List<ValidationDetails> GetValidationDetails()
        {
            var validationDetails = new List<ValidationDetails>();
            var properties = typeof(Flight).GetProperties();

            foreach (var property in properties)
            {
                var details = new ValidationDetails
                {
                    PropertyName = property.Name,
                    Type = property.PropertyType.Name
                };

                var attributes = property.GetCustomAttributes(typeof(ValidationAttribute), true);
                foreach (ValidationAttribute attribute in attributes)
                {
                    details.Constraints.Add(attribute.ErrorMessage);
                }

                validationDetails.Add(details);
            }

            return validationDetails;
        }
    }
}
