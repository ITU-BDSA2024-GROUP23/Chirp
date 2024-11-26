using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly ChirpDBContext _context;

    public UserRepository(ChirpDBContext context)
    {
        _context = context;
    }

    #region Queries
        public async Task<List<User>> GetFollowers(User user)
    {
        var query = _context.Followers
            .Where(f => f.FolloweeId == user.Id)
            .Select(f => f.FollowerUser);
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<User>> GetFollowing(User user)
    {
        var query = _context.Followers
            .Where(f => f.FollowerId == user.Id)
            .Select(f => f.FolloweeUser);
        var result = await query.ToListAsync();
        return result;
    }
    public async Task<User> GetUserByString(string userString)
    {
        var query = _context.Users
            .Where(u => u.UserName == userString || u.Email == userString);
        var result = await query.FirstOrDefaultAsync();
        if (result == null)
        {
            throw new Exception("User not found");
        }
        return result;
    }
    #endregion

    #region Commands

    public async Task DeleteUser(User user)
    {
        User userToForget = (await _context.Users
            .Where(u => u.Id == user.Id)
            .FirstOrDefaultAsync())!;
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