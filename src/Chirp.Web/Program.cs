using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddEnvironmentVariables("GH_");

        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString));
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<ICheepRepository, CheepRepository>();

        builder.Services.AddHsts(options => options.MaxAge = TimeSpan.FromDays(365));

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ChirpDBContext>();

        builder.Services.AddAuthentication().AddCookie().AddGitHub(options =>
        {
            options.ClientId = builder.Configuration["GH_CLIENT_ID"] ?? throw new Exception("GitHub client ID not found");
            options.ClientSecret = builder.Configuration["GH_CLIENT_ID"] ?? throw new Exception("GitHub client secret not found");
            options.CallbackPath = "/auth/github/";
            options.Scope.Add("user:email");
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
