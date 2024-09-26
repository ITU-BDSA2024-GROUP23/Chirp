using Chirp.DB;

public class CheepService : ICheepService
{
    DBFacade db = new();
    
    public List<CheepViewModel> GetCheeps()
    {
        return db.GetCheeps(10, 0);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        return db.GetCheepsFromAuthor(10, 0, author);
    }
}
