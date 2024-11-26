public interface IUserService
{
    public Task<UserDTO> GetUserDTO(string user);
    public Task<User> GetUserByString(string userString);
    public Task<bool> FollowUser(User follower, User followee);
    public Task<bool> UnfollowUser(User follower, User followee);
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

    public async Task<UserDTO> GetUserDTO(string user)
    {
        try
        {
            User targetUser = await _userRepository.GetUserByString(user);
            List<User> followers = await _userRepository.GetFollowers(targetUser);
            List<User> following = await _userRepository.GetFollowing(targetUser);
            return new UserDTO
            {
                UserName = targetUser.UserName,
                FollowersCount = followers.Count,
                FollowingCount = following.Count
            };
        }
        catch (Exception)
        {
            return new UserDTO { UserName = "User not found", FollowersCount = 0, FollowingCount = 0 };
        }
    }

    public async Task<User> GetUserByString(string userString)
    {
        return await _userRepository.GetUserByString(userString);
    }

    public async Task<bool> FollowUser(User follower, User followee)
    {
        if (follower == followee || follower == null || followee == null)
        {
            return false;
        }

        await _userRepository.FollowUser(follower, followee);
        return true;
    }

    public async Task<bool> UnfollowUser(User follower, User followee)
    {
        if (follower == followee || follower == null || followee == null)
        {
            return false;
        }

        await _userRepository.UnfollowUser(follower, followee);
        return true;
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
