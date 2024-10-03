using Microsoft.EntityFrameworkCore;

public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
        if(Database.EnsureCreated())
        {
            DbInitializer.SeedDatabase(this);
        }
    }
}
