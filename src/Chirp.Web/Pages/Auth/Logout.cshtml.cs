using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages.Auth;

[Authorize]
public class Logout : PageModel
{
    public void OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            TempData["alert-success"] = "Signed out";
            Response.Cookies.Delete(".AspNetCore.Cookies");
            Response.Redirect("/");
        }
        return;
    }
}