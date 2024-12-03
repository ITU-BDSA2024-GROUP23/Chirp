using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class PublicModel : TimelineModel
{
    public PublicModel(
        SignInManager<User> signInManager,
        IUserService userService,
        ICheepService cheepService)
        : base(signInManager, userService, cheepService)
    {
    }

    /// <summary>
    /// Handles HTTP GET requests to display a paginated list of "cheeps" (user-generated content).<br/>
    /// This method fetches followed users and retrieves the appropriate page of cheeps based on the query parameter.
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnGetAsync([FromQuery] int page = 1)
    {
        await GetFollowedUsers();
        int offset = page - 1;
        Cheeps = await _cheepService.GetCheeps(offset);
        return Page();
    }
}
