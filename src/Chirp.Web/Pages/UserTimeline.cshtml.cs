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
    /// Handles HTTP GET requests to display "cheeps" (user-generated content) for a specific user or email address. <br/>
    /// This method determines the source of the request (authenticated user, email, or username) and retrieves the<br/>
    /// corresponding cheeps, while also preparing user-specific information.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnGetAsync(string user, [FromQuery(Name = "page")] int page = 1)
    {
        page = Math.Max(0, page - 1);
        Regex emailRegex = new(@"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$");

        await GetFollowedUsers();

        if (User.Identity!.IsAuthenticated && User.Identity.Name == user)
        {
            await GetFollowedCheeps(page);
        }
        else if (emailRegex.IsMatch(user))
        {
            Cheeps = await _cheepService.GetCheepsFromEmail(user, page);
        }
        else
        {
            Cheeps = await _cheepService.GetCheepsFromUserName(user, page);
        }

        await PrepareUserInfo(user);

        return Page();
    }

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
        User? currentUser = await _signInManager.UserManager.GetUserAsync(User); // User is authenticated, so this should never be null - unless we delete the user entry from the database
        if (currentUser == null)
        {
            TempData["alert-error"] = "Your cookie has expired. Please log in again.";
            await _signInManager.SignOutAsync();
            RedirectToPage();
            return;
        }

        UserDTO? userDTO = currentUser.ToUserDTO();
        if (userDTO == null)
        {
            TempData["alert-error"] = "An error occurred.";
            await _signInManager.SignOutAsync();
            RedirectToPage();
            return;
        }

        var userCheeps = await _cheepService.GetCheepsFromUserName(userDTO.UserName, page);

        var followedCheeps = new List<CheepDTO>();
        foreach (UserDTO followee in Following)
        {
            followedCheeps.AddRange(await _cheepService.GetCheepsFromUserName(followee.UserName, page));
        }

        Cheeps = userCheeps
            .Concat(followedCheeps)
            .OrderByDescending(cheep => cheep.TimeStamp)
            .ToList();
    }
}
