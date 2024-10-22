using System.ComponentModel.DataAnnotations;

public class Cheep
{
    public int CheepId { get; set; }
    public required Author Author { get; set; }
    public required int AuthorId { get; set; }

    // Make sure that the text is not empty and not longer than 160 characters
    [Required]
    [StringLength(160)]
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
}
