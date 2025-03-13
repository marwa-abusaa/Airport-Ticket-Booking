namespace Airport_Ticket_Booking.Presentation.Menu;

public class MainMenu
{
    public void DisplayMenu()
    {

        Console.WriteLine("Welcome to the Airport Ticket Booking System");
        Console.WriteLine("Are you a Passenger or a Manager?");
        Console.WriteLine("1. Passenger");
        Console.WriteLine("2. Manager");
        Console.Write("Please enter your choice (1 or 2): ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                PassengerMenu.PassengerActions();
                break;
            case "2":
                ManagerMenu.ManagerActions();
                break;
            default:
                Console.WriteLine("Invalid choice. Please select 1 for Passenger or 2 for Manager.");
                break;
        }
    }
}