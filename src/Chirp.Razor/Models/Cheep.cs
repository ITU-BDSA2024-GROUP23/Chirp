public class Cheep
{
    public int CheepId { get; set; }
    public required Author Author { get; set; }
    public required string Text { get; set; }
    public DateTime Timestamp { get; set; }
}