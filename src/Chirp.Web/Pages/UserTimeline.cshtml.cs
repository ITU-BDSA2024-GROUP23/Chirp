using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class UserTimelineModel : TimelineModel
{
    public UserInfoDTO? userInfo;

    public UserTimelineModel(
        IUserService userService,
        ICheepService cheepService,
        SignInManager<User> signInManager) : base(signInManager, userService, cheepService)
    {
    }

    /// <summary>
    /// 1. Get the user's cheeps if the user is authenticated and the user is the same as the user in the URL<br/>
    /// 2. Get the cheeps from username if the user is not authenticated <br/>
    /// 3. Get the cheeps from an email if the user is not authenticated and the user is an email <br/>
    /// </summary>
    /// <param name="user">The user/email to get the cheeps from</param>
    /// <param name="page">The page number</param>
    public async Task<IActionResult> OnGetAsync(string user, [FromQuery(Name = "page")] int page = 1)
    {
        CurrentPage = page; // 1-based here but 0-based below. Bit confusing, so maybe we should change it to 0-based here too.
        page = Math.Max(0, page - 1);
        Regex emailRegex = new(@"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$");

        await GetFollowedUsers();

        //TODO: we need to refactor "TotalCheeps". Its being used in multiple places and is a bit ugly.

        if (User.Identity!.IsAuthenticated && User.Identity.Name == user)
        {
            await GetFollowedCheeps(page);
            TotalCheeps = await _cheepService.GetTotalCheepsUser(user, true);
        }
        else if (emailRegex.IsMatch(user))
        {
            Cheeps = await _cheepService.GetCheepsFromEmail(user, page);
            TotalCheeps = await _cheepService.GetTotalCheepsEmail(user, false);
        }
        else
        {
            Cheeps = await _cheepService.GetCheepsFromUserName(user, page);
            TotalCheeps = await _cheepService.GetTotalCheepsUser(user, false);
        }

        await PrepareUserInfo(user);

        return Page();
    }

    /// <summary>
    /// This method is called upon accessing the user timeline page. <br/>
    /// It is responsible for creating the user card, that shows the user's information. <br/>
    /// If a user is not found, it shows a dummy user card. <br/>
    /// </summary>
    private async Task PrepareUserInfo(string user)
    {
        userInfo = await _userService.GetUserInfoDTO(user);
        if (userInfo == null)
        {
            // empty user info - this is shown when a user accesses a user timeline that does not exist
            userInfo = new UserInfoDTO
            {
                UserName = "User not found",
                Email = "notfound",
                Cheeps = new List<CheepDTO>(),
                Followers = new List<UserDTO>(),
                Following = new List<UserDTO>()
            };
        }
    }

    private async Task GetFollowedCheeps(int page)
    {
        User? currentUser = await _signInManager.UserManager.GetUserAsync(User);
        if (currentUser == null)
        {
            TempData["alert-error"] = "Your cookie has expired. Please log in again.";
            await _signInManager.SignOutAsync();
            RedirectToPage();
            return;
        }

        var cheeps = await _cheepService.GetCheepsForUserAndFollowees(currentUser.UserName!, page);

        Cheeps = cheeps;
    }

}
