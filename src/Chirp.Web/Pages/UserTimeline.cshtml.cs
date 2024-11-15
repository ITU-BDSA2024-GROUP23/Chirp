using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class UserTimelineModel : TimelineModel
{
    public UserTimelineModel(ICheepRepository repository, SignInManager<User> signInManager) : base(repository, signInManager)
    {
    }

    public async Task<IActionResult> OnGetAsync(string user, [FromQuery(Name = "page")] int page = 1)
    {
        page = Math.Max(0, page - 1);
        string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        Regex regex = new(emailPattern);

        if (User.Identity!.IsAuthenticated && User.Identity.Name == user)
        {
            await GetFollowedCheeps(page);
        }
        else if (regex.IsMatch(user))
        {
            Cheeps = await _repository.GetCheepsFromEmail(user, page);
        }
        else
        {
            Cheeps = await _repository.GetCheepsFromUserName(user, page);
        }
        return Page();
    }

    private async Task GetFollowedCheeps(int page)
    {
        User currentUser = await _signInManager.UserManager.GetUserAsync(User) ?? throw new Exception("User not found"); // User is authenticated, so this should never be null
        var userCheeps = await _repository.GetCheepsFromUserName(currentUser.UserName, page);

        Following = await _repository.GetFollowing(currentUser);
        var followedCheeps = new List<CheepDTO>();
        
        foreach (User followee in Following)
        {
            followedCheeps.AddRange(await _repository.GetCheepsFromUserName(followee.UserName, page));
        }

        Cheeps = userCheeps
            .Concat(followedCheeps)
            .OrderByDescending(cheep => cheep.TimeStamp)
            .ToList();
    }
}
