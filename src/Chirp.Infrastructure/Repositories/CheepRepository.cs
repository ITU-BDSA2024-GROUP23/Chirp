using Microsoft.EntityFrameworkCore;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _context;
    private const int pageSize = 32;
    private const string defaultTimeStampFormat = "dd/MM/yyyy HH:mm:ss";

    public CheepRepository(ChirpDBContext context)
    {
        _context = context;
    }

    #region Queries
    public async Task<List<CheepDTO>> GetCheeps(int page = 0)
    {
        var query = _context.Cheeps
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(cheep => new CheepDTO(
                cheep.Author.UserName,
                cheep.Text,
                cheep.TimeStamp.ToString(defaultTimeStampFormat)
            ));
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<CheepDTO>> GetCheepsFromUserName(string userName, int page)
    {
        var query = _context.Cheeps
            .Where(cheep => cheep.Author.UserName == userName)
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(cheep => new CheepDTO(
                cheep.Author.UserName,
                cheep.Text,
                cheep.TimeStamp.ToString(defaultTimeStampFormat)
            ));
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<CheepDTO>> GetCheepsFromEmail(string email, int page)
    {
        var query = _context.Cheeps
            .Where(cheep => cheep.Author.Email == email)
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(cheep => new CheepDTO(
                cheep.Author.UserName,
                cheep.Text,
                cheep.TimeStamp.ToString(defaultTimeStampFormat)
            ));
        var result = await query.ToListAsync();
        return result;
    }

    public int GetNextCheepId()
    {
        var query = _context.Cheeps
            .OrderByDescending(cheep => cheep.CheepId)
            .Select(cheep => cheep.CheepId);
        var result = query.FirstOrDefault() + 1;
        return result;
    }

    public async Task<User> GetUserByString(string userString)
    {
        var query = _context.Users
            .Where(u => u.UserName == userString || u.Email == userString);
        var result = await query.FirstOrDefaultAsync();
        return result;
    }

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
    #endregion

    #region Commands

    public async Task CreateCheep(User user, string message)
    {
        User author = await _context.Users
            .Where(u => u.UserName == user.UserName)
            .FirstOrDefaultAsync();

        Cheep newCheep = new Cheep
        {
            CheepId = GetNextCheepId(),
            Author = author,
            Text = message,
            TimeStamp = DateTime.Now
        };
        await _context.Cheeps.AddAsync(newCheep);
        await _context.SaveChangesAsync();
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
