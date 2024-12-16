using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Follower
{
    [ForeignKey("FollowerId")]
    [Required]
    public required User FollowerUser { get; set; }
    [ForeignKey("FolloweeId")]
    [Required]
    public required User FolloweeUser { get; set; }
    [Required]
    public required DateTime FollowDate { get; set; }
}
