using Microsoft.EntityFrameworkCore;

public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpContext(DbContextOptions<ChirpContext> options) : base(options)
    {

    }

}
