

public interface ICheepRepository
{
    public Task<List<CheepDTO>> GetCheeps(int page);
    public Task<List<CheepDTO>> GetCheepsFromUserName(string name, int page);
    public Task<List<CheepDTO>> GetCheepsFromUserName(string name);
    public Task<List<CheepDTO>> GetCheepsFromEmail(string email, int page);
    public Task CreateCheep(User user, string message);
    public int GetNextCheepId();
    public Task<User> GetUserByString(string userString);
    public Task FollowUser(User follower, User followee);
    public Task UnfollowUser(User follower, User followee);
    public Task<List<User>> GetFollowers(User user);
    public Task<List<User>> GetFollowing(User user);
    public Task ForgetMe(User user);
}

