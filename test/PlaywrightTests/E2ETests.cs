using Microsoft.Playwright.NUnit;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;

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
    public async Task Test_Layout()
    {
        await Page.GotoAsync("http://localhost:5273/");
        await Page.GetByRole(AriaRole.Contentinfo).ClickAsync();
        await Page.GetByRole(AriaRole.Navigation).ClickAsync();
        await Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Chirp Logo" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Chirp!" }).ClickAsync();
        await Page.Locator("#navbarNav").ClickAsync();
        await Page.GetByRole(AriaRole.Navigation).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
    }

    [Test]
    public async Task RegisterPageLoads()
    {
        await Page.GotoAsync("http://localhost:5273/Identity/Account/Register");
        await Page.GetByRole(AriaRole.Heading, new() { Name = "Register a new account on" }).ClickAsync();
        await Page.GetByRole(AriaRole.Heading, new() { Name = "Create a new account" }).ClickAsync();
        await Page.Locator("#registerForm div").Filter(new() { HasText = "Username" }).ClickAsync();
        await Page.GetByPlaceholder("Email").ClickAsync();
        await Page.Locator("#registerForm div").Filter(new() { HasText = "Email" }).ClickAsync();
        await Page.Locator("#registerForm div").Nth(2).ClickAsync();
        await Page.GetByPlaceholder("Password", new() { Exact = true }).ClickAsync();
        await Page.Locator("#registerForm div").Filter(new() { HasText = "Confirm Password" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register", Exact = true }).ClickAsync();
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }
}