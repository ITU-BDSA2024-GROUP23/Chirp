using System.Globalization;
using CsvHelper;

namespace SimpleDB;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly static string filePath = Path.GetFullPath(Path.GetTempPath(), "/simple-database.csv");
    private static CSVDatabase<T>? instance;

    public static CSVDatabase<T> GetInstance()
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }

        instance ??= new CSVDatabase<T>();
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
}
