using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Like
{
    [ForeignKey("CheepId")]
    [Required]
    public required Cheep Post { get; set; }
    [ForeignKey("Id")]
    [Required]
    public required User Liker { get; set; }
}
