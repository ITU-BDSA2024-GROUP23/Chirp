using Chirp.DB;

public class CheepService : ICheepService
{
    private readonly DBFacade _db;
    private readonly int _pageSize = 10;

    // Dependency injection of DBFacade
    public CheepService(DBFacade db)
    {
        this._db = db;
    }

    public List<CheepViewModel> GetCheeps(int page)
    {
        return _db.GetCheeps(_pageSize, page);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        int offset = (page - 1) * _pageSize;
        return _db.GetCheepsFromAuthor(_pageSize, offset, author);
    }
}
