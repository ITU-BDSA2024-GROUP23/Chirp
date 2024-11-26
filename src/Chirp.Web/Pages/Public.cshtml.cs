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

    public async Task<IActionResult> OnGetAsync([FromQuery] int page = 1)
    {
        await GetFollowedUsers();
        int offset = page - 1;
        Cheeps = await _cheepService.GetCheeps(offset);
        return Page();
    }
}
