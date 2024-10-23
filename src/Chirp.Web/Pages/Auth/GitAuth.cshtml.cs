using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages.Auth;

[Authorize]
public class GitAuth : PageModel
{
    public void OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            TempData["alert-success"] = "Signed in as " + User.Identity.Name;
            Response.Redirect("/");
        }
        return;
    }
}