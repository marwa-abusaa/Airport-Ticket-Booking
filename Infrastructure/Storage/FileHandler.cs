using CsvHelper;
using System.Globalization;

namespace Airport_Ticket_Booking.Infrastructure.Storage;

public class FileHandler
{
    public List<T> ReadFromFile<T>(string filePath)
    {
        List<T> records = new List<T>();

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            records = csv.GetRecords<T>().ToList();
        }

        return records;
    }

    public void WriteToFile<T>(List<T> records, string filePath)
    {
        using (var writer = new StreamWriter(filePath, append: false))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
    }
}