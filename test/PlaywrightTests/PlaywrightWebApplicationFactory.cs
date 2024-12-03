using System.Data.Common;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class PlaywrightWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private IHost? _host;

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

            services.Remove(dbContextDescriptor!);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

            services.Remove(dbConnectionDescriptor!);

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
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
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
        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

        // Create and start the Kestrel server before the test server,  
        // otherwise due to the way the deferred host builder works    
        // for minimal hosting, the server will not get "initialized    
        // enough" for the address it is listening on to be available.    
        // See https://github.com/dotnet/aspnetcore/issues/33846.    

        _host = builder.Build();
        _host.Start();

        var server = _host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();

        ClientOptions.BaseAddress = addresses!.Addresses
            .Select(x => new Uri(x))
            .Last();

        // Return the host that uses TestServer, rather than the real one.  
        // Otherwise the internals will complain about the host's server    
        // not being an instance of the concrete type TestServer.    
        // See https://github.com/dotnet/aspnetcore/pull/34702.   

        testHost.Start();
        return testHost;
    }

    public string ServerAddress
    {
        get
        {
            EnsureServer();
            return ClientOptions.BaseAddress.ToString();
        }
    }

    private void EnsureServer()
    {
        if (_host is null)
        {
            // This forces WebApplicationFactory to bootstrap the server  
            using var _ = CreateDefaultClient();
        }
    }

    protected override void Dispose(bool disposing)
    {
        _host?.Dispose();
    }
}
