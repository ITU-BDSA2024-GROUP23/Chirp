using System.ComponentModel.DataAnnotations;

public class Cheep
{
    public required int CheepId { get; set; }
    public required User Author { get; set; }
    public required DateTime TimeStamp { get; set; }
    [Required]
    [StringLength(160)]
    public required string Text { get; set; }
}
