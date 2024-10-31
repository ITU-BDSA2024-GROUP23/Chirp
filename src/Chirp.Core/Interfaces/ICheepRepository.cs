

public interface ICheepRepository
{
    public Task<List<CheepDTO>> GetCheeps(int page);
    public Task<List<CheepDTO>> GetCheepsFromUserName(string name, int page);
    public Task<List<CheepDTO>> GetCheepsFromEmail(string email, int page);
    //public Task<bool> CreateUser(string name, string email);
    //public Task CreateCheep(string name, string message);
    public int GetNextCheepId();
    //public int GetNextAuthorId();
}

