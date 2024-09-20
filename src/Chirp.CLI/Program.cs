using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DocoptNet;


namespace Chirp.CLI
{
    class Program
    {
        private const string Usage = @"Chirp CLI.

Usage:
    chirp read
    chirp cheep
    chirp (-h | --help)

Options:
    -h --help     Show this screen.";

        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5141")
        };

        static Program()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        static void Main(string[] args)
        {
            var arguments = new Docopt().Apply(Usage, args, version: "Chirp CLI 0.1", exit: true);

            if (arguments["read"].IsTrue)
            {
                GetCheeps().Wait();
            }
            else if (arguments["cheep"].IsTrue)
            {
                PostCheep().Wait();
            }
            else if (arguments["--help"].IsTrue || arguments["-h"].IsTrue)
            {
                Console.WriteLine(Usage);
            }
        }

        static async Task GetCheeps()
        {
            var cheeps = await client.GetFromJsonAsync<List<Cheep>>("cheeps");

            foreach (var atom in cheeps)
            {
                Console.WriteLine($"{atom.Author} @: {atom.Message}: {atom.Timestamp}");
            }
        }

        static async Task PostCheep()
        {
            Console.WriteLine("Write your Cheep!:");

            string message = Console.ReadLine();

            var newCheep = new Cheep(Environment.UserName, message, DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            var post = await client.PostAsJsonAsync("cheep", newCheep);

        }

    }
}
public record Cheep(string Author, string Message, long Timestamp);