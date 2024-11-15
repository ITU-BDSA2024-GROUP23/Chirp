using Chirp.Web.Pages.Shared.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDTO> Cheeps { get; set; } = new();
    public List<User> Following { get; set; } = new();
    [BindProperty]
    public CheepBoxModel CheepBox { get; set; } = new();
    private readonly SignInManager<User> _signInManager;

    public PublicModel(ICheepRepository repository, SignInManager<User> signInManager)
    {
        _repository = repository;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnGetAsync([FromQuery] int page = 1)
    {
        await GetFollowedUsers();
        int offset = page - 1;
        Cheeps = _repository.GetCheeps(offset).Result.ToList();
        return Page();
    }

    private async Task GetFollowedUsers()
    {
        if(User.Identity.IsAuthenticated)
        {
            User currentUser = await _signInManager.UserManager.GetUserAsync(User);
            Following = _repository.GetFollowing(currentUser).Result.ToList();
        }
    }
    public IActionResult OnPost()
    {
        User? user = _signInManager.UserManager.GetUserAsync(User).Result;
        if (user == null)
        {
            TempData["alert-error"] = "You must be logged in to post a cheep!";
            return RedirectToPage("Public");
        }
        if (!ModelState.IsValid)
        {
            string errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)); // got this from https://stackoverflow.com/a/4934712
            TempData["alert-error"] = errors;
            return RedirectToPage("Public");
        }
        _repository.CreateCheep(user, CheepBox.Message ?? throw new InvalidOperationException("Cheep message is null!")); // we should never get to the exception because of the validation
        TempData["alert-success"] = "Cheep posted successfully!";
        return RedirectToPage("Public");
    }
    public IActionResult OnPostFollow(string followee)
    {
        User? follower = _signInManager.UserManager.GetUserAsync(User).Result;
        User? followeeUser = _repository.GetUserByString(followee).Result;
        _repository.FollowUser(follower, followeeUser);
        TempData["alert-success"] = "User followed successfully!";
        return RedirectToPage();
    }

    public IActionResult OnPostUnfollow(string followee)
    {
        User? follower = _signInManager.UserManager.GetUserAsync(User).Result;
        User? followeeUser = _repository.GetUserByString(followee).Result;
        _repository.UnfollowUser(follower, followeeUser);
        TempData["alert-success"] = "User unfollowed successfully!";
        return RedirectToPage();
    }

    public bool IsFollowing(string author)
    {
        return Following.Any(f => f.UserName == author);
    }
}
