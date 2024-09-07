using DocoptNet;
using SimpleDB;

namespace Chirp.CLI
{
    class Program
    {
        private readonly static string filePath = "cheepDB.csv";
        private const string Usage = @"Chirp CLI.

Usage:
    chirp read
    chirp cheep <cheep>...
    chirp (-h | --help)

Options:
    -h --help     Show this screen.";

        static void Main(string[] args)
        {
            var arguments = new Docopt().Apply(Usage, args, version: "Chirp CLI 0.1", exit: true);
            if(arguments["read"].IsTrue) {
                ReadCheeps();
            }
            else if(arguments["cheep"].IsTrue) {
                WriteCheep(string.Join(" ", args[1..]));
            }
        }

        static void ReadCheeps()
        {
            var db = new CSVDatabase<Cheep>(filePath);
            IEnumerable<Cheep> cheeps = db.Read();
            foreach(var cheep in cheeps)
            {
                Console.WriteLine(cheep.ToString());
            }   
        }
        static void WriteCheep(string message)
        {
            var db = new CSVDatabase<Cheep>(filePath);
            var date = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            db.Store(new Cheep(Environment.UserName, message, date));
            Console.WriteLine("Cheeped: " + message);
        }
    }

    // we have to refactor this to a separate file 
    public record Cheep(string Author, string Message, long Timestamp)
    {
        public override string ToString()
        {
            return Author + " @ " + UnixTimeStampToDateTime(Timestamp) + ": " + Message;
        }

        private static string UnixTimeStampToDateTime(long timestamp)
        {
            DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
            return dateTime.ToString("dd-MM-yyyy HH:mm:ss");
        }
    }
}