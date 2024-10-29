using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString));
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<ICheepRepository, CheepRepository>();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "GitHub";
        })
        .AddCookie()
        .AddGitHub(options =>
        {
            if (builder.Environment.IsDevelopment())
            {
                options.ClientId = builder.Configuration["GitHub_ClientId"] ?? throw new Exception("GitHub_ClientId not found in configuration");
                options.ClientSecret = builder.Configuration["GitHub_ClientSecret"] ?? throw new Exception("GitHub_ClientSecret not found in configuration");
            }
            else
            {
                options.ClientId = Environment.GetEnvironmentVariable("GH_CLIENT_ID") ?? throw new Exception("GITHUB_CLIENT_ID not found in environment variables");
                options.ClientSecret = Environment.GetEnvironmentVariable("GH_CLIENT_SECRET") ?? throw new Exception("GITHUB_CLIENT_SECRET not found in environment variables");
            }
            options.CallbackPath = "/auth/github/";
            // options.Scope.Add("user:email"); TODO: Implement
        });

        var app = builder.Build();

        app.UseCookiePolicy(new CookiePolicyOptions {
            MinimumSameSitePolicy = SameSiteMode.Lax
        });

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();

        app.MapRazorPages();
        app.Run();
    }
}
