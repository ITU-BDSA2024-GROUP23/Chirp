using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

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
        Cheeps = _repository.GetCheeps(offset).Result.ToList();
        return Page();
    }

    public IActionResult OnPost(string cheep)
    {
        // for now we will just hardcode the author id, until we implement user auth
        _repository.CreateCheep(13, cheep);
        return RedirectToPage("Public");
    }
}
