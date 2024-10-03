

public interface ICheepRepository
{
    public Task<List<CheepDTO>> GetCheeps(int page = 0);
    public Task<List<CheepDTO>> GetCheepsFromAuthor(Author author, int page =0);
}

