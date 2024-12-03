using Microsoft.CodeAnalysis.CSharp.Syntax;
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

    public async Task<UserDTO> GetUserByString(string userString) // TODO: Consider checked exception or nullable?
    {
        var query = _context.Users.Where(u => u.UserName == userString || u.Email == userString);
        User user = await query.FirstOrDefaultAsync() ?? throw new Exception("User not found");
        UserDTO result = user.ToUserDTO() ?? throw new Exception("Invalid user");
        return result;
    }

    public async Task<User> GetUserObjectByString(string userString)
    {
        var query = _context.Users.Where(u => u.UserName == userString || u.Email == userString);
        User user = await query.FirstOrDefaultAsync() ?? throw new Exception("User not found");
        return user;
    }

    // Returns null if no such user exists in the database.
    // Many commands must handle this null-value so maybe
    // This should just throw, and the commands can propagate
    // the exception?
    private async Task<User?> UserFromDTO(UserDTO dto) {
        var query = _context.Users.Where(u => u.Id == dto.Id);
        User? user = await query.FirstOrDefaultAsync();
        return user;
    }
    #endregion

    #region Commands

    public async Task<bool> DeleteUser(UserDTO user)
    {
        User? userToForget = await UserFromDTO(user);
        if (userToForget == null) return false;

        // Remove cheeps
        var cheepsToRemove = _context.Cheeps.Where(c => c.Author.Id == user.Id);
        _context.Cheeps.RemoveRange(cheepsToRemove);

        // Remove followers
        var followersToRemove = _context.Followers.Where(f => f.FolloweeId == user.Id || f.FollowerId == user.Id);
        _context.Followers.RemoveRange(followersToRemove);

        // Remove user
        _context.Users.Remove(userToForget);
        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<bool> FollowUser(UserDTO follower, UserDTO followee)
    {
        User? followerUser = await UserFromDTO(follower);
        User? followeeUser = await UserFromDTO(followee);
        if (followerUser == null || followeeUser == null) return false;

        Follower newFollowRelation = new Follower
        {
            FollowerId = followerUser.Id,
            FollowerUser = followerUser,
            FolloweeId = followeeUser.Id,
            FolloweeUser = followeeUser,
            FollowDate = DateTime.Now
        };

        await _context.Followers.AddAsync(newFollowRelation);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnfollowUser(UserDTO follower, UserDTO followee)
    {
        var query = _context.Followers.Where(f => f.FollowerId == follower.Id && f.FolloweeId == followee.Id);
        Follower? relationToRemove = await query.FirstOrDefaultAsync();
        if (relationToRemove == null) return false;

        _context.Followers.Remove(relationToRemove);
        await _context.SaveChangesAsync();

        return true;
    }
    #endregion
}
