using AspNetCoreGeneratedDocument;

using Chirp.Web.Pages.Shared.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public abstract class TimelineModel : PageModel
{
    protected readonly IUserService _userService;
    protected readonly ICheepService _cheepService;
    public List<CheepDTO> Cheeps { get; set; } = new();
    protected List<User> Following { get; set; } = new();
    [BindProperty]
    public CheepBoxModel CheepBox { get; set; } = new();
    protected readonly SignInManager<User> _signInManager;

    public TimelineModel(
        SignInManager<User> signInManager,
        IUserService userService,
        ICheepService cheepService)
    {
        _signInManager = signInManager;
        _userService = userService;
        _cheepService = cheepService;
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
        await _cheepService.CreateCheep(user, CheepBox.Message ?? throw new InvalidOperationException("Cheep message is null!")); // we should never get to the exception because of the validation
        TempData["alert-success"] = "Cheep posted successfully!";
        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostFollow(string followee)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            TempData["alert-error"] = "You must be logged in to follow someone!";
            return RedirectToPage();
        }

        User? follower = _signInManager.UserManager.GetUserAsync(User).Result;
        User followeeUser = _userService.GetUserByString(followee).Result;

        if (follower == followeeUser)
        {
            TempData["alert-error"] = "You can't follow yourself!";
            return RedirectToPage();
        }

        await _userService.FollowUser(follower, followeeUser);
        TempData["alert-success"] = $"You are now following {followeeUser.UserName}!";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUnfollow(string followee)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            TempData["alert-error"] = "You must be logged in to unfollow someone!";
            return RedirectToPage();
        }

        User? follower = _signInManager.UserManager.GetUserAsync(User).Result;
        User followeeUser = _userService.GetUserByString(followee).Result;

        if (follower == followeeUser)
        {
            TempData["alert-error"] = "You can't unfollow yourself!";
            return RedirectToPage();
        }

        await _userService.UnfollowUser(follower, followeeUser);
        TempData["alert-success"] = $"You are no longer following {followeeUser.UserName}!";
        return RedirectToPage();
    }

    public bool IsFollowing(string author)
    {
        return Following.Any(f => f.UserName == author);
    }

    protected async Task GetFollowedUsers()
    {
        if (_signInManager.IsSignedIn(User))
        {
            User currentUser = await _signInManager.UserManager.GetUserAsync(User);
            if (currentUser == null)
            {
                TempData["alert-error"] = "Your cookie has expired. Please log in again.";
                await _signInManager.SignOutAsync();
                RedirectToPage();
                return;
            }
            Following = _userService.GetFollowing(currentUser).Result.ToList();
        }
    }
}

