using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ChirpDBContext : IdentityDbContext<User>
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Follower> Followers { get; set; }
    public DbSet<Like> Likes { get; set; }

    /// <summary>
    /// This is the constructor for our DBContext.
    /// It will check if there are any pending migrations and apply them if there are. <br/>
    /// It will also seed the database with some initial data if there are no Cheeps. <br/>
    /// The purpose of this is to ensure the database is always in a consistent state.
    /// </summary>
    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
        if (Database.GetPendingMigrations().Any())
        {
            Database.Migrate();
        }

        if (!Cheeps!.Any())
        {
            DbInitializer.SeedDatabase(this);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Follower>()
            .HasKey("FollowerId", "FolloweeId");

        modelBuilder.Entity<Like>()
            .HasKey("Id", "CheepId");
    }
}
