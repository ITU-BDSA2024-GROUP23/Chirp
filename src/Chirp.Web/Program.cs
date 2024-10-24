using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
        builder.Services.AddSession(options => {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        
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
            if(builder.Environment.IsDevelopment())
            {
                options.ClientId = builder.Configuration["GitHub:ClientId"] ?? throw new Exception("GitHub:ClientId not found in configuration");
                options.ClientSecret = builder.Configuration["GitHub:ClientSecret"] ?? throw new Exception("GitHub:ClientSecret not found in configuration");
            }
            else
            {
                options.ClientId = Environment.GetEnvironmentVariable("GitHub_ClientId") ?? throw new Exception("GitHub_ClientId not found in environment variables");
                options.ClientSecret = Environment.GetEnvironmentVariable("GitHub_ClientSecret") ?? throw new Exception("GitHub_ClientSecret not found in environment variables");
            }
            options.CallbackPath = "/auth/github/";
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
