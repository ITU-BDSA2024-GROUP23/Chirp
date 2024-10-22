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

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapRazorPages();
        app.Run();
    }
}
