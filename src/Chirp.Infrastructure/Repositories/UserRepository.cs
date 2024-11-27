using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly ChirpDBContext _context;

    public UserRepository(ChirpDBContext context)
    {
        _context = context;
    }

    #region Queries
    public async Task<List<UserDTO>> GetFollowers(UserDTO user)
    {
        var query = _context.Followers
            .Where(f => f.FolloweeId == user.Id)
            .Select(f => f.FollowerUser.ToUserDTO())
            .OfType<UserDTO>();
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<UserDTO>> GetFollowing(UserDTO user)
    {
        var query = _context.Followers
            .Where(f => f.FollowerId == user.Id)
            .Select(f => f.FolloweeUser.ToUserDTO())
            .OfType<UserDTO>();
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<UserDTO> GetUserByString(string userString) // TODO: Consider checked exception
    {
        var query = _context.Users.Where(u => u.UserName == userString || u.Email == userString);
        User user = await query.FirstOrDefaultAsync() ?? throw new Exception("User not found");
        UserDTO result = user.ToUserDTO() ?? throw new Exception("Invalid user");
        return result;
    }
    #endregion

    #region Commands

    public async Task DeleteUser(UserDTO user)
    {
        var query = _context.Users.Where(u => u.Id == user.Id);
        User? userToForget = await query.FirstOrDefaultAsync();

        if (userToForget != null)
        {
            // Remove cheeps
            var cheepsToRemove = _context.Cheeps.Where(c => c.Author.Id == user.Id);
            _context.Cheeps.RemoveRange(cheepsToRemove);

            // Remove followers
            var followersToRemove = _context.Followers.Where(f => f.FolloweeId == user.Id || f.FollowerId == user.Id);
            _context.Followers.RemoveRange(followersToRemove);

            // Remove user
            _context.Users.Remove(userToForget);
            await _context.SaveChangesAsync();
        }
    }
    public async Task FollowUser(User follower, User followee)
    {
        Follower newFollower = new Follower
        {
            FollowerId = follower.Id,
            FollowerUser = follower,
            FolloweeId = followee.Id,
            FolloweeUser = followee,
            FollowDate = DateTime.Now
        };
        await _context.Followers.AddAsync(newFollower);
        await _context.SaveChangesAsync();
    }

    public async Task UnfollowUser(User follower, User followee)
    {
        Follower? followerToRemove = await _context.Followers
            .Where(f => f.FollowerId == follower.Id && f.FolloweeId == followee.Id)
            .FirstOrDefaultAsync();
        if (followerToRemove != null)
        {
            _context.Followers.Remove(followerToRemove);
            await _context.SaveChangesAsync();
        }
    }
    #endregion
}
