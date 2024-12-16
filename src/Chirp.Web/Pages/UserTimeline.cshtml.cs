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
