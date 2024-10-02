using Microsoft.EntityFrameworkCore;

public class CheepContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public CheepContext(DbContextOptions<CheepContext> options ) : base(options)
    {
        
    }

}

public class Cheep
{
    public int CheepId { get; set; }
    public required Author Author { get; set; }
    public required string Text { get; set; }
    public DateTime Timestamp { get; set; }
}

public class Author
{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<Cheep> Cheeps { get; set; } = new List<Cheep>();
}