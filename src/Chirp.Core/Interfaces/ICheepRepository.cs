

public interface ICheepRepository
{
    public Task<List<CheepDTO>> GetCheeps(int page);
    public Task<List<CheepDTO>> GetCheepsFromName(string name, int page);
    public Task<List<CheepDTO>> GetCheepsFromEmail(string email, int page);
    public Task CreateUser(string name, string email);
    public Task CreateCheep(int authorId, string message);
    public int GetNextCheepId();
    public int GetNextAuthorId();
}

