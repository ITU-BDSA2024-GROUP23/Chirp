public interface IUserRepository
{
    public Task<UserDTO> GetUserByString(string userString);
    public Task<bool> FollowUser(UserDTO follower, UserDTO followee);
    public Task<bool> UnfollowUser(UserDTO follower, UserDTO followee);
    public Task<List<UserDTO>> GetFollowers(UserDTO user);
    public Task<List<UserDTO>> GetFollowing(UserDTO user);
    public Task<bool> DeleteUser(UserDTO user);
}
