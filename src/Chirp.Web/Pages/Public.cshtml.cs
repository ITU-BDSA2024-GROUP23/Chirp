using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class PublicModel : TimelineModel
{
    public PublicModel(ICheepRepository repository, SignInManager<User> signInManager) : base(repository, signInManager)
    {
    }

    public async Task<IActionResult> OnGetAsync([FromQuery] int page = 1)
    {
        await GetFollowedUsers();
        int offset = page - 1;
        Cheeps = _repository.GetCheeps(offset).Result.ToList();
        return Page();
    }
}
