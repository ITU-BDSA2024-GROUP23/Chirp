using Microsoft.Playwright.NUnit;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
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
        await Expect(Page.GetByRole(AriaRole.Heading)).ToContainTextAsync("Public Timeline");
        await Expect(Page.GetByRole(AriaRole.Navigation)).ToContainTextAsync("Chirp!");
        await Expect(Page.GetByRole(AriaRole.List)).ToContainTextAsync("public timeline");
        await Expect(Page.GetByRole(AriaRole.List)).ToContainTextAsync("register");
        await Expect(Page.GetByRole(AriaRole.List)).ToContainTextAsync("login");
        await Expect(Page.GetByRole(AriaRole.Contentinfo)).ToContainTextAsync("Chirp — An ASP.NET Application");
    }

    [Test]
    public async Task Test_Register()
    {
        await Page.GotoAsync("http://localhost:5273/Identity/Account/Register");
        await Expect(Page.Locator("h2")).ToContainTextAsync("Create a new account");
        await Expect(Page.Locator("#registerForm")).ToContainTextAsync("Username");
        await Expect(Page.Locator("#registerForm")).ToContainTextAsync("Email");
        await Expect(Page.Locator("#registerForm")).ToContainTextAsync("Password");
        await Expect(Page.Locator("#registerForm")).ToContainTextAsync("Confirm Password");
        await Expect(Page.Locator("#registerSubmit")).ToContainTextAsync("Register");
        await Expect(Page.Locator("h3")).ToContainTextAsync("Use another service to register");
        await Expect(Page.Locator("button[name=\"provider\"]")).ToContainTextAsync("Register with GitHub");

    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }
}