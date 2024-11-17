using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class PlaywrightWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private IHost? host;

    /// <summary>
    /// This class overrides the default database configuration of the application to use an 
    /// in-memory SQLite database, ensuring that each test runs in isolation. <br/> <br/>
    /// Nearly all code below is copied from the Microsoft documentation. <br/>
    /// Source: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ChirpDBContext>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

            services.Remove(dbConnectionDescriptor);

            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<ChirpDBContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });
        });

        builder.UseEnvironment("Development");
    }

    /// <summary>
    /// This method creates a separate host for Kestrel, which is required for Playwright to work. <br/>
    /// The method also sets the base address of the client to the address assigned by Kestrel. <br/>
    /// Credits to Daniel Donbavand for the implementation. <br/> <br/>
    /// Link to the original implementation: <br/>
    /// https://danieldonbavand.com/2022/06/13/using-playwright-with-the-webapplicationfactory-to-test-a-blazor-application/
    /// </summary>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var testHost = builder.Build();
        var kestrelBuilder = builder.ConfigureWebHost(webBuilder => 
        {
            webBuilder.UseKestrel()
                     .UseUrls("http://localhost:5000"); 
        });

        host = kestrelBuilder.Build();
        host.Start();

        // Get the assigned port from Kestrel
        var server = host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();

        ClientOptions.BaseAddress = addresses!.Addresses  
            .Select(x => new Uri(x))  
            .Last(); 

        // Start and return the test host
        testHost.Start();
        return testHost;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            host?.Dispose();
        }
    }
}