public interface IUserService
{
    public Task<UserInfoDTO?> GetUserInfoDTO(string userStr);
    public Task<UserDTO> GetUserByString(string userStr);
    public Task<bool> FollowUser(UserDTO? follower, UserDTO? followee);
    public Task<bool> UnfollowUser(UserDTO? follower, UserDTO? followee);
    public Task<List<UserDTO>> GetFollowers(UserDTO user);
    public Task<List<UserDTO>> GetFollowing(UserDTO user);
    public Task<bool> DeleteUser(UserDTO? user);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICheepRepository _cheepRepository;

    public UserService(IUserRepository userRepository, ICheepRepository cheepRepository)
    {
        _cheepRepository = cheepRepository;
        _userRepository = userRepository;
    }

    public async Task<UserInfoDTO?> GetUserInfoDTO(string userStr)
    {
        try
        {
            UserDTO targetUser = await _userRepository.GetUserByString(userStr);
            List<CheepDTO> cheeps = await _cheepRepository.GetCheepsFromUserName(targetUser.UserName);
            List<UserDTO> followers = await _userRepository.GetFollowers(targetUser);
            List<UserDTO> following = await _userRepository.GetFollowing(targetUser);
            return new UserInfoDTO
            {
                UserName = targetUser.UserName,
                Email = targetUser.Email,
                Cheeps = cheeps,
                Followers = followers,
                Following = following
            };
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<UserDTO> GetUserByString(string userStr)
    {
        return await _userRepository.GetUserByString(userStr);
    }

    public async Task<bool> FollowUser(UserDTO? follower, UserDTO? followee)
    {
        if (follower == null || followee == null || follower.UserName == followee.UserName)
        {
            return false;
        }

        return await _userRepository.FollowUser(follower, followee);
    }

    public async Task<bool> UnfollowUser(UserDTO? follower, UserDTO? followee)
    {
        if (follower == null || followee == null || follower.UserName == followee.UserName)
        {
            return false;
        }

        return await _userRepository.UnfollowUser(follower, followee);
    }

    public async Task<List<UserDTO>> GetFollowers(UserDTO user)
    {
        return await _userRepository.GetFollowers(user);
    }

    public async Task<List<UserDTO>> GetFollowing(UserDTO user)
    {
        return await _userRepository.GetFollowing(user);
    }

    public async Task<bool> DeleteUser(UserDTO? user)
    {
        if (user == null)
        {
            return false;
        }
        return await _userRepository.DeleteUser(user);
    }
}
