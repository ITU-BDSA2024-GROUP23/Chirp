using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class RegisterModel : PageModel
{
    private readonly ICheepRepository _repository;

    public RegisterModel(ICheepRepository repository)
    {
        _repository = repository;
    }

    public ActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost(string name, string email)
    {
        return Page();
    }
}
