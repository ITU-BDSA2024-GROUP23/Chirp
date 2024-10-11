using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class CreateUserModel : PageModel
{
    private readonly ICheepRepository _repository;

    public CreateUserModel(ICheepRepository repository)
    {
        _repository = repository;
    }

    public ActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost(string name, string email)
    {
        _repository.CreateUser(name, email);
        return RedirectToPage("Public");
    }
}
