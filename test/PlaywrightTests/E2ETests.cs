using System.Diagnostics;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

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
    public async Task TestLayoutUI()
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
    public async Task TestRegisterUI()
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

    [Test]
    public async Task TestCheepUI()
    {
        await InitTestUser();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("Whats on your mind ropf? Cheep 160 characters max")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "logout [ropf]" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestCanLogin()
    {
        await InitTestUser();
        await Page.GotoAsync("http://localhost:5273/Identity/Account/Login");
        await Page.GetByPlaceholder("Email").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("Email").PressAsync("Tab");
        await Page.GetByPlaceholder("Password").FillAsync("LetM31n!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in", Exact = true }).ClickAsync();
        await Expect(Page.GetByText("Whats on your mind ropf? Cheep 160 characters max")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "logout [ropf]" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestCanLogout()
    {
        await InitTestUser();
        await Page.GotoAsync("http://localhost:5273/Identity/Account/Logout");
        await Expect(Page.GetByRole(AriaRole.Heading)).ToContainTextAsync("Log out of Chirp");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to log out" }).ClickAsync();
        await Expect(Page.GetByText("You have been logged out.")).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestAboutMeNavBar()
    {
        await InitTestUser();
        await Page.GotoAsync("http://localhost:5273/");
        await Page.GetByRole(AriaRole.Link, new() { Name = "about me" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "ropf (ropf@itu.dk)" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestAboutMeButtons()
    {
        await InitTestUser();
        await Page.GotoAsync("http://localhost:5273/Identity/Account/AboutMe");
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Download My Data" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Delete account" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task AboutMeFollowers()
    {
        await InitTestUser();
        await Page.GotoAsync("http://localhost:5273/Identity/Account/AboutMe");
        await Expect(Page.GetByText("0 Followers")).ToBeVisibleAsync();
        await Expect(Page.GetByText("0 Following")).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestAboutMeActivity()
    {
        await InitTestUser();
        await Page.GotoAsync("http://localhost:5273/Identity/Account/AboutMe");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Cheeping activity" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("No recent activity found.")).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestUserCanDeleteAccount()
    {
        await InitTestUser();
        await Page.GotoAsync("http://localhost:5273/Identity/Account/AboutMe");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete account" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Delete account" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Cancel" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Cancel" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete account" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Yes I am sure" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Yes I am sure" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Log in", Exact = true })).ToBeVisibleAsync();
    }

    [Test]
    public async Task AboutMeNotLoggedIn()
    {
        await Page.GotoAsync("http://localhost:5273/Identity/Account/AboutMe"); ;
        await Expect(Page.Locator("body")).ToContainTextAsync("You are not logged in.");
    }

    // This method is used to create a test user for the tests. - moved to a seperate method because an authed account is needed for multiple tests
    // NOTE: If the registrations fails, all tests dependent on this method will fail.
    private async Task InitTestUser()
    {
        await Page.GotoAsync("http://localhost:5273/Identity/Account/Register");
        await Page.GetByPlaceholder("Username").FillAsync("ropf");
        await Page.GetByPlaceholder("Username").PressAsync("Tab");
        await Page.GetByPlaceholder("Email").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("Email").PressAsync("Tab");
        await Page.GetByPlaceholder("Password", new() { Exact = true }).FillAsync("LetM31n!");
        await Page.GetByPlaceholder("Password", new() { Exact = true }).PressAsync("Tab");
        await Page.GetByPlaceholder("Confirm Password").FillAsync("LetM31n!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register", Exact = true }).ClickAsync();
    }

    [Test]
    public async Task FollowAndUnfollow_UserCard()
    {
        await InitTestUser();
        await Page.GotoAsync("http://localhost:5273/");
        await Page.Locator(".d-inline > .btn").First.ClickAsync();
        await Page.Locator(".text-decoration-none").First.ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Main)).ToContainTextAsync("1 Followers");
        await Page.Locator(".btn").First.ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Main)).ToContainTextAsync("0 Followers");
    }


    [Test]
    public async Task Following_ShowUp_MyTimeline()
    {
        await InitTestUser();
        await Page.GotoAsync("http://localhost:5273/");
        await Page.Locator(".d-inline > .btn").First.ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Main)).ToContainTextAsync("Jacqualine Gilcoine");
        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Main)).ToContainTextAsync("Jacqualine Gilcoine");
    }


    [Test]
    public async Task WriteCheep()
    {
        await InitTestUser();
        await Page.GotoAsync("http://localhost:5273/");
        await Page.GetByPlaceholder("Cheep something..").ClickAsync();
        await Page.GetByPlaceholder("Cheep something..").FillAsync("Hello what are you up to?");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Cheep" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Main)).ToContainTextAsync("Hello what are you up to?");

    }

    [Test]
    public async Task DeleteCheep()
    {
        await WriteCheep();
        await Expect(Page.GetByText("Hello what are you up to?")).ToBeVisibleAsync();
        await Page.Locator(".border-0").First.ClickAsync();
        await Expect(Page.GetByText("Hello what are you up to?")).Not.ToBeVisibleAsync();
    }


    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }
}
