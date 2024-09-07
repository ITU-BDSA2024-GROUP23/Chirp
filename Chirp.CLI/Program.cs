using DocoptNet;
using CsvHelper;
using System.Globalization;

namespace Chirp.CLI
{
    class Program
    {
        static readonly string filePath = "cheepDB.csv";
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
            if(!File.Exists(filePath))
            {
                Console.WriteLine("No cheeps found.");
                return;
            }

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var cheeps = csv.GetRecords<Cheep>();
            foreach (var cheep in cheeps)
            {
                Console.WriteLine(cheep.Author + " @ " + UnixTimeStampToDateTime(cheep.Timestamp) + ": " + cheep.Message);
            }
        }
        static void WriteCheep(string message)
        {
            var cheep = new Cheep(
                Author: Environment.UserName,
                Message: message,
                Timestamp: ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() // ugly for now
            );

            using var writer = new StreamWriter(filePath, true);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            if(!File.Exists(filePath))
            {
                Console.WriteLine("CSV file not found. Creating new file..");
                csv.WriteHeader<Cheep>();
                writer.WriteLine();
            }
            
            csv.WriteRecord(cheep);
            writer.WriteLine();

            Console.WriteLine("Cheep sent!");
            
        }

        // Modified version of https://stackoverflow.com/a/250400
        static string UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime.ToString("dd-MM-yyyy HH:mm:ss");
        }
    }

    public record Cheep(string Author, string Message, long Timestamp);
}