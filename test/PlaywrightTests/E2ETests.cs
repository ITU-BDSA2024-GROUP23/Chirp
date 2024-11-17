using Microsoft.Playwright.NUnit;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PlaywrightTests;

[TestFixture]
public class E2ETests : PageTest
{
    private CustomWebApplicationFactory<Program> _factory;
    private Process _serverProcess;

    [SetUp]
    public void Setup()
    {
        _factory = new CustomWebApplicationFactory<Program>();
    }


    [Test]
    public async Task CanAccessHomePage()
    {
        await Page.GotoAsync("localhost:5273");
        Assert.AreEqual("Playwright .NET", await Page.TitleAsync());
    }

    [TearDown]
    public void TearDown()
    {
        _serverProcess.Dispose();
    }
}