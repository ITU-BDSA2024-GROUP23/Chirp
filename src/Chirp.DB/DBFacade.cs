using System.Reflection;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileProviders;

namespace Chirp.DB;

public class DBFacade
{
    public static readonly string DEFAULT_DB_PATH = Path.Combine(Path.GetTempPath(), "chirp.db");
    private readonly SqliteConnection _connection;
    
    public DBFacade() 
    {
        string? customDBPath = Environment.GetEnvironmentVariable("CHIRPDBPATH");
        var dbPath = customDBPath ?? DEFAULT_DB_PATH

        _connection = new SqliteConnection($"Data Source={dbPath}");
        _connection.Open();

        // If database did not exist before connection.Open(),
        // create schema and fill with dummy data
        if (new FileInfo(dbPath).Length == 0)
        {
            ExecuteNonQueryFromEmbeddedScript("scripts/schema.sql");
            ExecuteNonQueryFromEmbeddedScript("scripts/dump.sql");
        }
    }

    public List<CheepViewModel> GetCheeps(int limit, int offset)
    {
        var sqlQuery = @"
            SELECT u.username, m.text, m.pub_date
            FROM message m
                JOIN user u ON m.author_id = u.user_id
            ORDER BY m.pub_date DESC
            LIMIT @limit OFFSET @offset
        ";

        var command = _connection.CreateCommand();
        command.CommandText = sqlQuery;
        command.Parameters.AddWithValue("@limit", limit);
        command.Parameters.AddWithValue("@offset", offset);

        return CheepsFromCommand(command);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(int limit, int offset, string author)
    {
        var sqlQuery = @"
            SELECT u.username, m.text, m.pub_date
            FROM message m
                JOIN user u ON m.author_id = u.user_id
            WHERE u.username=@author
            ORDER by m.pub_date desc
            LIMIT @limit OFFSET @offset
        ";

        var command = _connection.CreateCommand();
        command.CommandText = sqlQuery;
        command.Parameters.AddWithValue("@limit", limit);
        command.Parameters.AddWithValue("@offset", offset);
        command.Parameters.AddWithValue("@author", author);

        return CheepsFromCommand(command);
    }

    private static List<CheepViewModel> CheepsFromCommand(SqliteCommand command)
    {
        List<CheepViewModel> cheeps = new();

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var author = reader.GetString(0);
                var message = reader.GetString(1);
                var timestamp = UnixTimeStampToDateTimeString(reader.GetInt64(2));

                var cheep = new CheepViewModel(author, message, timestamp);
                cheeps.Add(cheep);
            }
        }

        return cheeps;
    }

    private void ExecuteNonQueryFromEmbeddedScript(string scriptPath)
    {
        var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        var scriptReader = embeddedProvider.GetFileInfo(scriptPath).CreateReadStream();
        var scriptStream = new StreamReader(scriptReader);

        var command = _connection.CreateCommand();
        command.CommandText = scriptStream.ReadToEnd();
        command.ExecuteNonQuery();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}
