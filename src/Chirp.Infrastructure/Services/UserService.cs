public interface IUserService
{
    public Task<User> GetUserByString(string userString);
    public Task FollowUser(User follower, User followee);
    public Task UnfollowUser(User follower, User followee);
    public Task<List<User>> GetFollowers(User user);
    public Task<List<User>> GetFollowing(User user);
    public Task DeleteUser(User user);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetUserByString(string userString)
    {
        return await _userRepository.GetUserByString(userString);
    }

    public async Task FollowUser(User follower, User followee)
    {
        await _userRepository.FollowUser(follower, followee);
    }

    public async Task UnfollowUser(User follower, User followee)
    {
        await _userRepository.UnfollowUser(follower, followee);
    }

    public async Task<List<User>> GetFollowers(User user)
    {
        return await _userRepository.GetFollowers(user);
    }

    public async Task<List<User>> GetFollowing(User user)
    {
        return await _userRepository.GetFollowing(user);
    }

    public async Task DeleteUser(User user)
    {
        await _userRepository.DeleteUser(user);
    }

}