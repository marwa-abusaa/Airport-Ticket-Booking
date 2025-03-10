using CsvHelper;
using System.Globalization;

public class FileHandler
{
    private readonly string _filePath;

    public FileHandler(string filePath)
    {
        _filePath = filePath;
    }

    public List<T> ReadFromFile<T>()
    {
        List<T> records = new List<T>();

        using (var reader = new StreamReader(_filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            records = csv.GetRecords<T>().ToList();
        }

        return records;
    }

    public void WriteToFile<T>(List<T> records)
    {
        using (var writer = new StreamWriter(_filePath, append: true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records); 
        }
    }
}
