using System.Net.Http.Headers;
using System.Net.Http.Json;
using DocoptNet;
using static Chirp.CLI.UserInterface;


namespace Chirp.CLI
{
    class Program
    {
#if (DEBUG)
        private const string DBWebServiceURL = "http://localhost:5000";
#else
        private const string DBWebServiceURL = "https://chirp-remote-db.azurewebsites.net";
#endif

        private static HttpClient client = new();
        private const string Usage = @"Chirp CLI.
        

Usage:
    chirp read
    chirp cheep <cheep>...
    chirp (-h | --help)

Options:
    -h --help     Show this screen.";

        static Program()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(DBWebServiceURL);
        }

        static void Main(string[] args)
        {
            var arguments = new Docopt().Apply(Usage, args, version: "Chirp CLI 0.1", exit: true)!;

            if (arguments["read"].IsTrue)
            {
                GetCheeps().Wait();
            }
            else if (arguments["cheep"].IsTrue)
            {
                PostCheep(string.Join(" ", args[1])).Wait();
            }
            else if (arguments["--help"].IsTrue || arguments["-h"].IsTrue)
            {
                Console.WriteLine(Usage);
            }
        }

        static async Task GetCheeps()
        {
            var cheeps = await client.GetFromJsonAsync<List<Cheep>>("cheeps");

            if (cheeps == null)
            {
                Console.WriteLine("No cheeps found.");
                return;
            }

            PrintCheeps(cheeps);
        }

        static async Task PostCheep(string message)
        {
            var newCheep = new Cheep(Environment.UserName, message, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            await client.PostAsJsonAsync("cheep", newCheep);
        }
    }
}
