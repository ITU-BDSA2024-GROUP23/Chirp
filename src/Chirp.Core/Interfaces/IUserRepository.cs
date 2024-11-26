public interface IUserRepository
{
    public Task<User> GetUserByString(string userString);
    public Task FollowUser(User follower, User followee);
    public Task UnfollowUser(User follower, User followee);
    public Task<List<User>> GetFollowers(User user);
    public Task<List<User>> GetFollowing(User user);
    public Task DeleteUser(User user);
}
