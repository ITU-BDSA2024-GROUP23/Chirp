using Microsoft.AspNetCore.Mvc.Testing;

namespace Chirp.Web.Tests;

public class IntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;

    public IntegrationTest(CustomWebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
        _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
        });
    }


    [Fact]
    public async void PublicTimelineAvailable()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains("Public Timeline", content);
    }

    [Theory]
    [InlineData("Helge")]
    public async void PrivateTimelineAvailable(string user)
    {
        var response = await _client.GetAsync($"/{user}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains($"{user}'s profile", content);
        Assert.Contains("Timeline", content);
    }

    [Theory]
    [InlineData("Random user")]
    public async void NoCheepsAvailable(string user)
    {
        var response = await _client.GetAsync($"/{user}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("There are no cheeps so far.", content);
    }
    /* TODO: This is deprecated due to our db not being seeded with data anymore
    [Fact]
    public async void DifferentPages()
    {
        var response = await _client.GetAsync("/?page=1");
        response.EnsureSuccessStatusCode();
        var content1 = await response.Content.ReadAsStringAsync();

        response = await _client.GetAsync("/?page=2");
        response.EnsureSuccessStatusCode();
        var content2 = await response.Content.ReadAsStringAsync();

        Assert.NotEqual(content1, content2);
    }
    */

}
