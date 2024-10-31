using Microsoft.EntityFrameworkCore;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _context;
    private readonly int pageSize = 32;
    private readonly string defaultTimeStampFormat = "dd/MM/yyyy HH:mm:ss";

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
    #endregion

    #region Commands

    /*

    public async Task<bool> CreateUser(string userName, string email)
    {
        // Check if user already exists
        bool userExists = await _context.Users.AnyAsync(user => user.Email == email || user.UserName == userName);
        if (userExists)
        {
            return false;
        }

        // Create new user
        User newAuthor = new User
        {
            AuthorId = GetNextAuthorId(), // not sure if this is adhering to the Command Query Separation principle - but it will be replaced anyway
            Name = name,
            Email = email,
            Cheeps = new List<Cheep>()
        };
        await _context.Authors.AddAsync(newAuthor);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task CreateCheep(string name, string message)
    {
        throw new NotImplementedException();
    }

    
    public async Task CreateCheep(string name, string message)
    {
        Author? author = await _context.Authors.FirstOrDefaultAsync(author => author.Name == name);
        //TODO: add error handling if author is not found

        // create new cheep
        Cheep newCheep = new Cheep
        {
            CheepId = GetNextCheepId(),
            Author = author,
            AuthorId = author.AuthorId,
            Text = message,
            TimeStamp = DateTime.Now
        };
        await _context.Cheeps.AddAsync(newCheep);
        await _context.SaveChangesAsync();
    }
    */

    #endregion
}
