public interface ICheepRepository
{
    public Task<List<CheepDTO>> GetCheeps(int page);
    public Task<List<CheepDTO>> GetCheepsFromUserName(string name, int page);
    public Task<List<CheepDTO>> GetCheepsFromUserName(string name);
    public Task<List<CheepDTO>> GetCheepsFromEmail(string email, int page);
    public Task<bool> CreateCheep(User user, string message);
    public int GetNextCheepId();
    public Task<bool> DeleteCheep(int cheepId);
    public Task<Cheep?> GetCheep(int cheepId);
    public Task<bool> LikeCheep(User liker, Cheep cheep);
    public Task<bool> UnlikeCheep(User unliker, Cheep cheep);
    public Task<bool> HasLiked(User user, int cheepId);
    public Task<int> GetLikes(int cheepId);
}
