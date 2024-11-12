using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ChirpDBContext : IdentityDbContext<User>
{
    public DbSet<Cheep> Cheeps { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
        if (Database.EnsureCreated())
        {
            //DbInitializer.SeedDatabase(this);
        }
    }
}
