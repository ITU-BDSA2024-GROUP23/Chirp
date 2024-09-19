using System.Globalization;
using CsvHelper;

namespace SimpleDB;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly static string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "SimpleDB", "data", "cheepDB.csv")); // Surely there is a better way to do this?
    private static CSVDatabase<T>? instance;

    private CSVDatabase()
    {
    }

    public static CSVDatabase<T> GetInstance()
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Database file not found.");
        }

        if (instance == null)
        {
            instance = new CSVDatabase<T>();
        }
        return instance;
    }
    public IEnumerable<T> Read(int? limit = null)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        IEnumerable<T> records = csv.GetRecords<T>();
        List<T> recordList = new(records);
        return recordList;
    }

    public void Store(T record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }
        using var writer = new StreamWriter(filePath, true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        writer.WriteLine();
        csv.WriteRecord(record);
    }

    public void ResetTestDB()
    {
        //Write "id" as header
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteHeader<T>();
    }
}