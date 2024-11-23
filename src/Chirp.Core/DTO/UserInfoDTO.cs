public record UserInfoDTO
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
}
