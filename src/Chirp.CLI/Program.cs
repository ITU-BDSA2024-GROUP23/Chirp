using DocoptNet;
using SimpleDB;
using static Chirp.CLI.UserInterface;

namespace Chirp.CLI
{
    class Program
    {
        private readonly static string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "SimpleDB", "data", "cheepDB.csv")); // Surely there is a better way to do this?
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
            if (arguments["read"].IsTrue)
            {
                ReadCheeps();
            }
            else if (arguments["cheep"].IsTrue)
            {
                WriteCheep(string.Join(" ", args[1..]));
            }
        }

        static void ReadCheeps()
        {
            var db = new CSVDatabase<Cheep>(filePath);
            IEnumerable<Cheep> cheeps = db.Read();
            PrintCheeps(cheeps);
        }

        static void WriteCheep(string message)
        {
            var db = new CSVDatabase<Cheep>(filePath);
            var date = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            db.Store(new Cheep(Environment.UserName, message, date));
            Console.WriteLine("Cheeped: " + message);
        }
    }
}