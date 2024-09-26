using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Chirp.DB;

public class DBFacade
{
    SqliteConnection connection;
    
    // Establish connection
    public DBFacade() {
        string dbPath = Path.Combine(Path.GetTempPath(), "chirp.db");
        string? dbPathEnvVar = Environment.GetEnvironmentVariable("CHIRPDBPATH");

        if(!String.IsNullOrEmpty(dbPathEnvVar))
        {
            dbPath = dbPathEnvVar;
        }

        // Execute init.sh in /scripts **TEMPORARY**
        var initScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "scripts", "init.sh");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = initScriptPath,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();
    }

    public List<CheepViewModel> GetCheeps(int page)
    {
        var sqlQuery = @$"
        SELECT u.username, m.text, m.pub_date
            FROM message m
                JOIN user u ON m.author_id = u.user_id
            ORDER BY m.pub_date DESC
            LIMIT 10 OFFSET {page*10}
        ";

        //@$"SELECT u.username, m.text, m.pub_date FROM message m JOIN user u ON m.author_id = u.user_id ORDER by m.pub_date desc";

        var command = connection.CreateCommand();
        command.CommandText = sqlQuery;

        List<CheepViewModel> cheeps = new();

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                cheeps.Add(new CheepViewModel(reader.GetString(0), reader.GetString(1), reader.GetInt64(2)));
            }
        }
        return cheeps;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(int page, string author)
    {
        var sqlQuery = @$"
        SELECT u.username, m.text, m.pub_date
            FROM message m
                JOIN user u ON m.author_id = u.user_id
            WHERE u.username='{author}'
            ORDER by m.pub_date desc
            LIMIT 10 OFFSET {page*10}
        ";

        var command = connection.CreateCommand();
        command.CommandText = sqlQuery;

        List<CheepViewModel> cheeps = new();

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                cheeps.Add(new CheepViewModel(reader.GetString(0), reader.GetString(1), reader.GetInt64(2)));
            }
        }
        return cheeps;
    }
}

public record CheepViewModel(string Author, string Message, long Timestamp);
