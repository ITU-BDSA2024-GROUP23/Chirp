using Chirp.DB;

public class CheepService : ICheepService
{
    private readonly DBFacade db;

    // Dependency injection of DBFacade
    public CheepService(DBFacade db)
    {
        this.db = db;
    }
    
    public List<CheepViewModel> GetCheeps()
    {
        return db.GetCheeps(10, 0);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        return db.GetCheepsFromAuthor(10, 0, author);
    }
}
