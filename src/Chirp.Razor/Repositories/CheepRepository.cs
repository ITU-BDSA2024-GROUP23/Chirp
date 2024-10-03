public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _context;

    public CheepRepository(ChirpDBContext context)
    {
        _context = context;
    }

    public async Task GetCheeps(int page = 0)
    {
        
    }

    public async Task GetCheepsFromAuthor(Author author, int page = 0)
    {
        throw new NotImplementedException();
    }
}
