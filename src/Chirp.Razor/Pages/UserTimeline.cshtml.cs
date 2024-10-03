using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDTO> Cheeps { get; set; }

    public UserTimelineModel(ICheepRepository repository)
    {
        _repository = repository;
        Cheeps = new();
    }

    public ActionResult OnGet(string author, [FromQuery(Name = "page")] int page = 1)
    {
        int offset = page - 1;
        Cheeps = _repository.GetCheepsFromAuthor(author, offset).Result.ToList();
        return Page();
    }
}
