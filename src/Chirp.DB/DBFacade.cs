using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Chirp.DB;

public class DBFacade
{
    SqliteConnection connection;
    
    // Establish connection - remember to run the scripts in "Chirp.DB/scripts" to create the database
    public DBFacade() {
        string dbPath = Path.Combine(Path.GetTempPath(), "chirp.db");
        string? dbPathEnvVar = Environment.GetEnvironmentVariable("CHIRPDBPATH");

        if(!String.IsNullOrEmpty(dbPathEnvVar))
        {
            dbPath = dbPathEnvVar;
        }

        connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();
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

        var command = connection.CreateCommand();
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

        var command = connection.CreateCommand();
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
                string author = reader.GetString(0);
                string message = reader.GetString(1);
                string timestamp = UnixTimeStampToDateTimeString(reader.GetInt64(2));

                CheepViewModel cheep = new(author, message, timestamp);
                cheeps.Add(cheep);
            }
        }
        return cheeps;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}

public record CheepViewModel(string Author, string Message, string Timestamp);
