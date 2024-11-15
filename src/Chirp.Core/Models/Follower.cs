using System.ComponentModel.DataAnnotations;

public class Follower
{
    [Required]
    public required string FollowerId { get; set; }
    [Required]
    public required User FollowerUser { get; set; }
    [Required]
    public required string FolloweeId { get; set; }
    [Required]
    public required User FolloweeUser { get; set; }
    [Required]
    public required DateTime FollowDate { get; set; }
}
