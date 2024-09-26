public record CheepViewModel (string Author, string Message, long Timestamp);

public interface ICheepService {
    List<CheepViewModel> GetCheeps();
    List<CheepViewModel> GetCheepsFromAuthor(string author);
}