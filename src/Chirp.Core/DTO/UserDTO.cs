public record UserDTO
{
    public required string UserName { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
}