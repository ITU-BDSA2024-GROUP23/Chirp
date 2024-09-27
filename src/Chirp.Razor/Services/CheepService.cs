using Chirp.DB;

public class CheepService : ICheepService
{
    private readonly DBFacade db;
    private readonly int pageSize = 10; // Number of cheeps to show per page

    // Dependency injection of DBFacade
    public CheepService(DBFacade db)
    {
        this.db = db;
    }
    
    public List<CheepViewModel> GetCheeps(int page)
    {
        return db.GetCheeps(pageSize, page);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        return db.GetCheepsFromAuthor(pageSize, 0, author);
    }
}
