using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public record Cheep(string Author, string Message, long Timestamp);

public class Program
{
    public static async Task Main(string[] args)
    {
        // Define the base URL for the API
        var baseURL = "http://localhost:5141"; // Ensure this is the correct port where your server is running
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        // Read all Cheeps from the database (GET request)
        await GetCheeps(client);

        // Ask the user to input the message for the new Cheep
        Console.Write("Enter your cheep message: ");
        string message = Console.ReadLine();

        // Create and post a new Cheep (POST request)
        var newCheep = new Cheep(Environment.UserName, message, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        await PostCheep(client, newCheep);

        // Read all Cheeps again to verify the new post was stored
        await GetCheeps(client);
    }

    // GET request to fetch all cheeps
    private static async Task GetCheeps(HttpClient client)
    {
        try
        {
            var cheeps = await client.GetFromJsonAsync<List<Cheep>>("cheeps");

            if (cheeps != null)
            {
                foreach (var cheep in cheeps)
                {
                    Console.WriteLine($"{cheep.Author} @, {cheep.Message}, {cheep.Timestamp}");
                }
            }
            else
            {
                Console.WriteLine("No cheeps found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching cheeps: {ex.Message}");
        }
    }

    // POST request to create a new cheep
    private static async Task PostCheep(HttpClient client, Cheep newCheep)
    {
        try
        {
            var response = await client.PostAsJsonAsync("cheep", newCheep);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Cheep posted successfully!");
            }
            else
            {
                Console.WriteLine($"Failed to post cheep. Status Code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error posting cheep: {ex.Message}");
        }
    }
}