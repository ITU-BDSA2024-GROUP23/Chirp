public interface IUserRepository
{
    public Task<UserDTO> GetUserByString(string userString);
    public Task FollowUser(UserDTO follower, UserDTO followee);
    public Task UnfollowUser(UserDTO follower, UserDTO followee);
    public Task<List<UserDTO>> GetFollowers(UserDTO user);
    public Task<List<UserDTO>> GetFollowing(UserDTO user);
    public Task DeleteUser(UserDTO user);
}
