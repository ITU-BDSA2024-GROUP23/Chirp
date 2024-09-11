namespace Chirp.CLI;

public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            Console.WriteLine(cheep.ToString());
        }
    }

    public record Cheep(string Author, string Message, long Timestamp)
    {
        public override string ToString()
        {
            return Author + " @ " + UnixTimeStampToDateTime(Timestamp) + ": " + Message;
        }

        public static string UnixTimeStampToDateTime(long timestamp)
        {
            DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
            return dateTime.ToString("dd-MM-yyyy HH:mm:ss");
        }
    }
}