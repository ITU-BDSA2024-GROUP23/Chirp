using Chirp.Infrastructure.Data;

public class Author
{
    public int AuthorId { get; set; }
    public User UserId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<Cheep> Cheeps { get; set; } = new List<Cheep>();
}
