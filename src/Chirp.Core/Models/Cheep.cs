using System.ComponentModel.DataAnnotations;

public class Cheep
{
    public required int CheepId { get; set; }
    public required User Author { get; set; }

    // Make sure that the text is not empty and not longer than 160 characters
    [Required]
    [StringLength(160)]
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }

    public CheepDTO? ToCheepDTO()
    {
        if (Author.UserName == null)
        {
            return null;
        }

        return new CheepDTO(
            Id: CheepId,
            Author: Author.UserName,
            Text: Text,
            TimeStamp: TimeStamp.ToString()
        );
    }
}
