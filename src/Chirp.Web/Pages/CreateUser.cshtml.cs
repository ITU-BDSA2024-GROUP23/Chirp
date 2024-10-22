using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

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

    public async Task<IActionResult> OnPost(string name, string email)
    {
        var result = await _repository.CreateUser(name, email);

        //if the user was created successfully, redirect to the public page
        if (result)
        {
            TempData["alert-success"] = "Account created successfully!";
            return RedirectToPage("Public");
        }

        TempData["alert-error"] = "The name or email is already in use..";
        return Page();
    }
}
