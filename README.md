# Airport Ticket Booking System

## Objective
Develop a .NET console application for an airport ticket booking system. This application enables passengers to book flight tickets and allows a manager to manage the bookings.

## Data Storage
- The system uses the **file system** as the data storage layer.

## Features

### For Passengers:
1. **Book a Flight**  
   - Select a flight based on various search parameters.  
   - Choose a class (Economy, Business, First Class) with different pricing.  

2. **Search for Available Flights**  
   - Filter by:
     - Price  
     - Departure Country  
     - Destination Country  
     - Departure Date  
     - Departure Airport  
     - Arrival Airport  
     - Class  

3. **Manage Bookings**  
   - Cancel a booking  
   - Modify a booking  
   - View personal bookings  

### For the Manager:
1. **Filter Bookings**  
   - Filter based on:
     - Flight  
     - Price  
     - Departure Country  
     - Destination Country  
     - Departure Date  
     - Departure Airport  
     - Arrival Airport  
     - Passenger  
     - Class  

2. **Batch Flight Upload**  
   - Import a list of flights into the system via CSV.  

3. **Validate Imported Flight Data**  
   - Ensure correctness of imported flight details.  
   - Return detailed error reports to fix issues.  

4. **Dynamic Model Validation**  
   - Generate validation constraints dynamically for each field.  
  

## Requirements
- .NET Framework / .NET Core  
- C# Development Environment  

