

public interface ICheepRepository
{
    public Task<List<CheepDTO>> GetCheeps(int page = 1);
    public Task<List<CheepDTO>> GetCheepsFromAuthor(string author, int page = 1);
}

