public interface ICheepService
{
    public Task<List<CheepDTO>> GetCheeps(int page);
    public Task<List<CheepDTO>> GetCheepsFromUserName(string name, int page);
    public Task<List<CheepDTO>> GetCheepsFromUserName(string name);
    public Task<List<CheepDTO>> GetCheepsFromEmail(string email, int page);
    public Task CreateCheep(User user, string message);
    public int GetNextCheepId();
    public Task<bool> DeleteCheep(int cheepId);
    public Task<bool> LikeCheep(User liker, int cheepId);
    public Task<bool> UnlikeCheep(User unliker, int cheepId);
    public Task<bool> HasLiked(User user, int cheepId);
    public Task<int> GetLikes(int cheepId);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository _cheepRepository;

    public CheepService(ICheepRepository cheepRepository)
    {
        _cheepRepository = cheepRepository;
    }

    public async Task<List<CheepDTO>> GetCheeps(int page)
    {
        return await _cheepRepository.GetCheeps(page);
    }

    public async Task<List<CheepDTO>> GetCheepsFromUserName(string name, int page)
    {
        return await _cheepRepository.GetCheepsFromUserName(name, page);
    }

    public async Task<List<CheepDTO>> GetCheepsFromUserName(string name)
    {
        return await _cheepRepository.GetCheepsFromUserName(name);
    }

    public async Task<List<CheepDTO>> GetCheepsFromEmail(string email, int page)
    {
        return await _cheepRepository.GetCheepsFromEmail(email, page);
    }

    public async Task CreateCheep(User user, string message)
    {
        await _cheepRepository.CreateCheep(user, message);
    }

    public int GetNextCheepId()
    {
        return _cheepRepository.GetNextCheepId();
    }

    public async Task<bool> DeleteCheep(int cheepId)
    {
        return await _cheepRepository.DeleteCheep(cheepId);
    }

    public async Task<bool> LikeCheep(User liker, int cheepId)
    {
        Cheep? cheep = await _cheepRepository.GetCheep(cheepId);
        if (cheep == null)
        {
            return false;
        }

        return await _cheepRepository.LikeCheep(liker, cheep);
    }

    public async Task<bool> UnlikeCheep(User unliker, int cheepId)
    {
        Cheep? cheep = await _cheepRepository.GetCheep(cheepId);
        if (cheep == null)
        {
            return false;
        }

        return await _cheepRepository.UnlikeCheep(unliker, cheep);
    }

    public async Task<bool> HasLiked(User user, int cheepId)
    {
        return await _cheepRepository.HasLiked(user, cheepId);
    }

    public async Task<int> GetLikes(int cheepId)
    {
        return await _cheepRepository.GetLikes(cheepId);
    }
}
