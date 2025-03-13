
using System.ComponentModel.DataAnnotations;

namespace Airport_Ticket_Booking.Validation;

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime date)
        {
            switch (date.Kind)
            {
                case DateTimeKind.Utc:
                    return date > DateTime.UtcNow;
                case DateTimeKind.Local:
                    return date.ToUniversalTime() > DateTime.UtcNow;
                default:
                    return DateTime.SpecifyKind(date, DateTimeKind.Utc) > DateTime.UtcNow;
            } 
        }
        return false;
    }
}

