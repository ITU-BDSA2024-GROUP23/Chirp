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
    protected List<UserDTO> Following { get; set; } = new();
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
            return ShowError("You must be logged in to post a cheep!");
        }

        if (!ModelState.IsValid)
        {
            string errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)); // got this from https://stackoverflow.com/a/4934712
            return ShowError(errors);
        }

        await _cheepService.CreateCheep(user, CheepBox.Message ?? throw new InvalidOperationException("Cheep message is null!")); // we should never get to the exception because of the validation

        TempData["alert-success"] = "Cheep posted successfully!";
        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostFollow(string followee)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return ShowError("You must be logged in to follow someone!");
        }

        User? follower = await _signInManager.UserManager.GetUserAsync(User);
        if (follower == null)
        {
            return await ShowErrorAndSignOut("Please log in again.");
        }

        UserDTO? followerDTO = follower.ToUserDTO();
        UserDTO followeeDTO = await _userService.GetUserByString(followee);

        bool success = await _userService.FollowUser(followerDTO, followeeDTO);
        if (!success)
        {
            return ShowError("You can't follow yourself!");
        }

        TempData["alert-success"] = $"You are now following {followeeDTO.UserName}!";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUnfollow(string followee)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return ShowError("You must be logged in to unfollow someone!");
        }

        User? follower = await _signInManager.UserManager.GetUserAsync(User);
        if (follower == null)
        {
            return await ShowErrorAndSignOut("Please log in again.");
        }

        UserDTO? followerDTO = follower.ToUserDTO();
        UserDTO followeeDTO = await _userService.GetUserByString(followee);

        bool success = await _userService.UnfollowUser(followerDTO, followeeDTO);
        if (!success)
        {
            return ShowError("You can't unfollow yourself!");
        }

        TempData["alert-success"] = $"You are no longer following {followeeDTO.UserName}!";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostLike(int cheepId) 
    {
        User? liker = await _signInManager.UserManager.GetUserAsync(User);

        if (liker == null || User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return ShowError("You must be logged in to interact with posts!");
        }

        if (!await _cheepService.LikeCheep(liker, cheepId)) 
        {
            return ShowError("Something went wrong!");
        }

        TempData["alert-success"] = $"You liked the post";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUnlike(int cheepId) 
    {
        User? unliker = await _signInManager.UserManager.GetUserAsync(User);

        if (unliker == null || User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return ShowError("You must be logged in to interact with posts!");
        }

        if (!await _cheepService.UnlikeCheep(unliker, cheepId)) {
            return ShowError("Something went wrong!");
        }
        
        TempData["alert-success"] = $"You no longer like this post!";
        return RedirectToPage();
    }

    public bool IsFollowing(string author)
    {
        return Following.Any(f => f.UserName == author);
    }

    public async Task<bool> HasLiked(int cheepId) 
    {
        User? user = await _signInManager.UserManager.GetUserAsync(User);

        if (user == null || User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return false;
        }

        return await _cheepService.HasLiked(user, cheepId);
    }

    public async Task<int> GetLikes(int cheepId) 
    {
        return await _cheepService.GetLikes(cheepId);
    }

    protected async Task GetFollowedUsers()
    {
        if (_signInManager.IsSignedIn(User))
        {
            User? currentUser = await _signInManager.UserManager.GetUserAsync(User);
            if (currentUser == null)
            {
                await ShowErrorAndSignOut("Please log in again.");
                return;
            }

            var currentUserDTO = currentUser.ToUserDTO();
            if (currentUserDTO == null)
            {
                await ShowErrorAndSignOut("Please log in again.");
                return;
            }

            Following = _userService.GetFollowing(currentUserDTO).Result.ToList();
        }
    }


    public async Task<IActionResult> OnPostDeleteCheep(int cheepId)
    {
        bool success = await _cheepService.DeleteCheep(cheepId);
        if (!success)
        {
            TempData["alert-error"] = "Cheep not found!";
            return RedirectToPage();
        }
        TempData["alert-success"] = "Cheep deleted successfully!";
        return RedirectToPage();
    }

    private IActionResult ShowError(string errorMsg, string? pageName = null)
    {
        TempData["alert-error"] = errorMsg;
        return RedirectToPage(pageName);
    }

    private async Task<IActionResult> ShowErrorAndSignOut(string errorMsg, string? pageName = null)
    {
        await _signInManager.SignOutAsync();
        return ShowError(errorMsg, pageName);
    }
}

