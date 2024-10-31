using Chirp.Infrastructure.Data;

public class Author
{
    public User AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<Cheep> Cheeps { get; set; } = new List<Cheep>();
}
