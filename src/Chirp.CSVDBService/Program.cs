using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "SimpleDB", "data", "cheepDB.csv")); // Surely there is a better way to do this?

var db = CSVDatabase<Cheep>.GetInstance(filePath);

app.MapGet("/cheeps", () =>
{
    return db.Read();
});

app.MapPost("/cheep", (Cheep cheep) =>
{
    db.Store(cheep);
});

app.Run();

public record Cheep(string Author, string Message, long Timestamp);
