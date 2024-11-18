using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ChirpDBContext : IdentityDbContext<User>
{
    public required DbSet<Cheep> Cheeps { get; set; }
    public required DbSet<Follower> Followers { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Entity<Follower>()
            .HasKey(f => new { f.FollowerId, f.FolloweeId });

        modelBuilder.Entity<Follower>()
            .HasOne(f => f.FollowerUser)
            .WithMany(u => u.Following)
            .HasForeignKey(f => f.FollowerId);

        modelBuilder.Entity<Follower>()
            .HasOne(f => f.FolloweeUser)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FolloweeId);
    }
}
