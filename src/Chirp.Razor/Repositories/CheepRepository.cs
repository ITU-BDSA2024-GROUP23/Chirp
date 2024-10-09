using Microsoft.EntityFrameworkCore;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _context;
    private readonly int pageSize = 32;

    public CheepRepository(ChirpDBContext context)
    {
        _context = context;
    }

    public async Task<List<CheepDTO>> GetCheeps(int page = 0)
    {
        var query = _context.Cheeps
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(cheep => new CheepDTO(
                cheep.Author.Name,
                cheep.Text,
                cheep.TimeStamp.ToString("dd/MM/yyyy HH:mm:ss")
            ));
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<CheepDTO>> GetCheepsFromAuthor(string author, int page = 0)
    {
        var query = _context.Cheeps
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Where(cheep => cheep.Author.Name == author)
            .Select(cheep => new CheepDTO(
                cheep.Author.Name,
                cheep.Text,
                cheep.TimeStamp.ToString("dd/MM/yyyy HH:mm:ss")
            ));
        var result = await query.ToListAsync();
        return result;
    }
}
