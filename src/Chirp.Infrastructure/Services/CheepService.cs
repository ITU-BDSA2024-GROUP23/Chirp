public interface ICheepService
{
    public Task<List<CheepDTO>> GetCheeps(int page);
    public Task<List<CheepDTO>> GetCheepsFromUserName(string name, int page);
    public Task<List<CheepDTO>> GetCheepsFromUserName(string name);
    public Task<List<CheepDTO>> GetCheepsFromEmail(string email, int page);
    public Task CreateCheep(User user, string message);
    public int GetNextCheepId();
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
}