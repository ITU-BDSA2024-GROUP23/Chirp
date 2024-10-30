using Chirp.Infrastructure.Data;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Load database connection via configuration
        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString));
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<ICheepRepository, CheepRepository>();

        //session
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ChirpDBContext>();

        //auth
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "GitHub";
        })
        .AddCookie()
        .AddGitHub(options =>
        {
            var clientId = builder.Environment.IsDevelopment()
                ? builder.Configuration["GitHub_ClientId"]
                : Environment.GetEnvironmentVariable("GitHub_ClientId");
                
            var clientSecret = builder.Environment.IsDevelopment()
                ? builder.Configuration["GitHub_ClientSecret"]
                : Environment.GetEnvironmentVariable("GitHub_ClientSecret");

            options.ClientId = clientId ?? throw new Exception("GitHub ClientId not found");
            options.ClientSecret = clientSecret ?? throw new Exception("GitHub ClientSecret not found");
            options.CallbackPath = "/auth/github/";
            options.Scope.Add("user:email");
        });

        var app = builder.Build();

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Lax // TODO: not sure if this is the best option but maybe look into it
        });

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();


        //auth
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();

        //run
        app.MapRazorPages();
        app.Run();
    }
}
