using Microsoft.Data.Sqlite;

namespace Chirp.DB;

public class DBFacade
{
    SqliteConnection connection;
    
    // Establish connection
    public DBFacade() {
        string dbPath = "/temp/chirp.db";
        string? pathEnvVar = Environment.GetEnvironmentVariable("CHIRPDBPATH");

        if(!String.IsNullOrEmpty(pathEnvVar))
        {
            dbPath = pathEnvVar;
        }

        connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();
    }


    // Return all chirps                    CHIRPDBPATH=./mychirp.db dotnet run

    // Return chirps by a specific user

}