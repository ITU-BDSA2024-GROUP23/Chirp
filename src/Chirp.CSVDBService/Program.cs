using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var db = CSVDatabase<Cheep>.GetInstance();

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
