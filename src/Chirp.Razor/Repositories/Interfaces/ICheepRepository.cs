

public interface ICheepRepository
{
    public Task<List<CheepDTO>> GetCheeps(int page);
    public Task<List<CheepDTO>> GetCheepsFromName(string name, int page);
    public Task<List<CheepDTO>> GetCheepsFromEmail(string email, int page);
}

