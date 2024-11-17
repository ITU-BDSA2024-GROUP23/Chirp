using Microsoft.Playwright.NUnit;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PlaywrightTests;

[TestFixture]
public class E2ETests : PageTest
{
    private PlaywrightWebApplicationFactory<Program> _factory;

    [SetUp]
    public async Task Init()
    {
        _factory = new PlaywrightWebApplicationFactory<Program>();
        _factory.CreateClient();
    }


    [Test]
    public async Task CanAccessHomePage()
    {
        await Page.GotoAsync("localhost:5000");
        Assert.AreEqual("Public Timeline | Chirp", await Page.TitleAsync());
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }
}