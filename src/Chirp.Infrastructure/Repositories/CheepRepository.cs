using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

using NuGet.ProjectModel;

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
            .Include(cheep => cheep.Author)
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(cheep => cheep.ToCheepDTO())
            .OfType<CheepDTO>();
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<CheepDTO>> GetCheepsFromUserName(string userName, int page)
    {
        var query = _context.Cheeps
            .Include(cheep => cheep.Author)
            .Where(cheep => cheep.Author.UserName == userName)
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(cheep => cheep.ToCheepDTO())
            .OfType<CheepDTO>();
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<CheepDTO>> GetCheepsFromUserName(string userName)
    {
        var query = _context.Cheeps
            .Include(cheep => cheep.Author)
            .Where(cheep => cheep.Author.UserName == userName)
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Take(pageSize)
            .Select(cheep => cheep.ToCheepDTO())
            .OfType<CheepDTO>();
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<CheepDTO>> GetCheepsFromEmail(string email, int page)
    {
        var query = _context.Cheeps
            .Include(cheep => cheep.Author)
            .Where(cheep => cheep.Author.Email == email)
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(cheep => cheep.ToCheepDTO())
            .OfType<CheepDTO>();
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

    public async Task<Cheep?> GetCheep(int cheepId) 
    {
        var query = _context.Cheeps
            .Where(cheep => cheep.CheepId == cheepId)
            .Select(cheep => cheep);
        var result = await query.FirstOrDefaultAsync();
        return result;
    } 

    public async Task<bool> HasLiked(User user, int cheepId)
    {
        var query = _context.Likes
            .Where(l => l.Liker.Id == user.Id && l.Post.CheepId == cheepId);
        var result = await query.FirstOrDefaultAsync();
        return result != null;
    }

    public async Task<int> GetLikes(int cheepId)
    {
        var query = _context.Likes
            .Where(l => l.Post.CheepId == cheepId);
        var result = await query.ToListAsync();
        return result.Count;
    }
    #endregion

    #region Commands

    public async Task<bool> CreateCheep(User user, string message)
    {
        User? author = await _context.Users
            .Where(u => u.UserName == user.UserName)
            .FirstOrDefaultAsync();

        if (author == null)
        {
            return false;
        }

        Cheep newCheep = new Cheep
        {
            CheepId = GetNextCheepId(),
            Author = author,
            Text = message,
            TimeStamp = DateTime.Now
        };
        await _context.Cheeps.AddAsync(newCheep);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteCheep(int cheepId)
    {
        Cheep? cheep = await _context.Cheeps
            .Where(c => c.CheepId == cheepId)
            .FirstOrDefaultAsync();
        if (cheep == null)
        {
            return false;
        }
        _context.Cheeps.Remove(cheep);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> LikeCheep(User liker, Cheep cheep)
    {
        Like newLike = new Like
        {
            Liker = liker,
            Post = cheep
        };

        _context.Likes.Add(newLike);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnlikeCheep(User unliker, Cheep cheep)
    {
        Like likeToRemove = new Like
        {
            Liker = unliker,
            Post = cheep
        };

        _context.Likes.Remove(likeToRemove);
        await _context.SaveChangesAsync();

        return true;
    }
    #endregion
}
