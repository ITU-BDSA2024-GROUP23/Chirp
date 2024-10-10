using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDTO> Cheeps { get; set; }

    public PublicModel(ICheepRepository repository)
    {
        _repository = repository;
        Cheeps = new();
    }

    public ActionResult OnGet([FromQuery] int page = 1)
    {
        int offset = page - 1;
        //_repository.CreateUser("Test User", "test@testingchirp.com");
        //_repository.CreateCheep(807521746, "This is a test cheep");
        Cheeps = _repository.GetCheeps(offset).Result.ToList();
        return Page();
    }
}
