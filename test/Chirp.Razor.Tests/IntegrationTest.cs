using Microsoft.AspNetCore.Mvc.Testing;

namespace Chirp.Razor.Tests;

public class TestsFixture : IDisposable
{
    public readonly HttpClient client;
    private readonly string? _envBefore; // Dont know if necessary
    private readonly string _dbCustomPath;
    private readonly WebApplicationFactory<Program> _waf;

    public TestsFixture() 
    {
        _envBefore = Environment.GetEnvironmentVariable("CHIRPDBPATH");
        _dbCustomPath = Path.Combine(Path.GetTempPath(), "integrationTests.db");
        _waf = new();

        Environment.SetEnvironmentVariable("CHIRPDBPATH", _dbCustomPath);
        File.Delete(_dbCustomPath);

        client = _waf.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
        });
    }

    public void Dispose() 
    {
        Environment.SetEnvironmentVariable("CHIRPDBPATH", _envBefore);
        client.Dispose();
        _waf.Dispose();
        File.Delete(_dbCustomPath);
    }
}

public class IntegrationTest : IClassFixture<TestsFixture>
{
    private readonly HttpClient _client;

    public IntegrationTest(TestsFixture fixture)
    {
        _client = fixture.client;
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
        Assert.Contains($"{user}'s Timeline", content);
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

    [Fact]
    public async void DifferentPages()
    {
        var response = await _client.GetAsync("/?page=0");
        response.EnsureSuccessStatusCode();
        var content1 = await response.Content.ReadAsStringAsync();

        response = await _client.GetAsync("/?page=1");
        response.EnsureSuccessStatusCode();
        var content2 = await response.Content.ReadAsStringAsync();

        Assert.NotEqual(content1, content2);
    }

}
