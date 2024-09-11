using System.Globalization;
using CsvHelper;

namespace SimpleDB;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string filePath;

    public CSVDatabase(string filePath)
    {
        this.filePath = filePath;
    }
    public IEnumerable<T> Read(int? limit = null)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Database file not found.");
            return Enumerable.Empty<T>();
        }

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        IEnumerable<T> records = csv.GetRecords<T>();
        List<T> recordList = new(records);
        return recordList;
    }

    public void Store(T record)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Database file not found.");
        }
        using var writer = new StreamWriter(filePath, true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        writer.WriteLine();
        csv.WriteRecord(record);
    }
}