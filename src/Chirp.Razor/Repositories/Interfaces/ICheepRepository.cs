

public interface ICheepRepository
{
    public Task GetCheeps(int page = 0);
    public Task GetCheepsFromAuthor(Author author, int page =0);
}

