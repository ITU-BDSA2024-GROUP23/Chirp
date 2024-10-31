using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages.Auth;

[Authorize]
public class GitAuth : PageModel
{
    private readonly ICheepRepository _repository;

    public GitAuth(ICheepRepository repository)
    {
        _repository = repository;
    }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            //TODO: handle these exceptions
            _ = User.FindFirst(ClaimTypes.Email)?.Value ?? throw new Exception("Email not found");
            _ = User.FindFirst(ClaimTypes.Name)?.Value ?? throw new Exception("Name not found");
            TempData["alert-success"] = "Signed in as " + User.Identity.Name;
            Response.Redirect("/");
        }
        return Page();
    }
}
