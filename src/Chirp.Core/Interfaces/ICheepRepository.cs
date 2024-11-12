

public interface ICheepRepository
{
    public Task<List<CheepDTO>> GetCheeps(int page);
    public Task<List<CheepDTO>> GetCheepsFromUserName(string name, int page);
    public Task<List<CheepDTO>> GetCheepsFromEmail(string email, int page);
    public Task CreateCheep(User user, string message);
    public int GetNextCheepId();
}

