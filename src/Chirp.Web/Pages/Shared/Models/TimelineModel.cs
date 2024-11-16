using Chirp.Web.Pages.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public abstract class TimelineModel : PageModel
{
    protected readonly ICheepRepository _repository;
    public List<CheepDTO> Cheeps { get; set; } = new();
    protected List<User> Following { get; set; } = new();
    [BindProperty]
    public CheepBoxModel CheepBox { get; set; } = new();
    protected readonly SignInManager<User> _signInManager;

    public TimelineModel(ICheepRepository repository, SignInManager<User> signInManager)
    {
        _repository = repository;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnPost()
    {
        User? user = _signInManager.UserManager.GetUserAsync(User).Result;
        if (user == null)
        {
            TempData["alert-error"] = "You must be logged in to post a cheep!";
            return RedirectToPage();
        }
        if (!ModelState.IsValid)
        {
            string errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)); // got this from https://stackoverflow.com/a/4934712
            TempData["alert-error"] = errors;
            return RedirectToPage();
        }
        await _repository.CreateCheep(user, CheepBox.Message ?? throw new InvalidOperationException("Cheep message is null!")); // we should never get to the exception because of the validation
        TempData["alert-success"] = "Cheep posted successfully!";
        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostFollow(string followee)
    {
        User? follower = _signInManager.UserManager.GetUserAsync(User).Result;
        User followeeUser = _repository.GetUserByString(followee).Result;
        await _repository.FollowUser(follower, followeeUser);
        TempData["alert-success"] = $"You are now following {followeeUser.UserName}!";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUnfollow(string followee)
    {
        User? follower = _signInManager.UserManager.GetUserAsync(User).Result;
        User followeeUser = _repository.GetUserByString(followee).Result;
        await _repository.UnfollowUser(follower, followeeUser);
        TempData["alert-success"] = $"You are no longer following {followeeUser.UserName}!";
        return RedirectToPage();
    }

    public bool IsFollowing(string author)
    {
        return Following.Any(f => f.UserName == author);
    }

    protected async Task GetFollowedUsers()
    {
        if (User.Identity!.IsAuthenticated)
        {
            User currentUser = await _signInManager.UserManager.GetUserAsync(User) ?? throw new Exception("User not found");
            Following = _repository.GetFollowing(currentUser).Result.ToList();
        }
    }
}

