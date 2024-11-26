﻿using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class UserTimelineModel : TimelineModel
{
    public UserDTO? userInfo;

    public UserTimelineModel(
        IUserService  userService, 
        ICheepService cheepService,
        SignInManager<User> signInManager) : base(signInManager, userService, cheepService)
    {
    }

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
        User targetUser = await _userService.GetUserByString(user);
        // TODO: Handle null targetUser - this is a cringe way to do it
        if (targetUser == null)
        {
            TempData["alert-error"] = "User not found";
            userInfo = new UserDTO
            {
                UserName = user,
                FollowersCount = 0,
                FollowingCount = 0
            };
        }
        else
        {
            userInfo = new UserDTO
            {
                UserName = targetUser.UserName,
                //find better way to do this - i imagine we need the list so you can click on followers/following and see who they are
                FollowersCount = (await _userService.GetFollowers(targetUser)).Count,
                FollowingCount = (await _userService.GetFollowing(targetUser)).Count
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
        var userCheeps = await _cheepService.GetCheepsFromUserName(currentUser.UserName, page);

        var followedCheeps = new List<CheepDTO>();
        foreach (User followee in Following)
        {
            followedCheeps.AddRange(await _cheepService.GetCheepsFromUserName(followee.UserName, page));
        }

        Cheeps = userCheeps
            .Concat(followedCheeps)
            .OrderByDescending(cheep => cheep.TimeStamp)
            .ToList();
    }
}
