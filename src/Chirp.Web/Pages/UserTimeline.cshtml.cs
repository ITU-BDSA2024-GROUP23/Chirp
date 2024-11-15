using System.Text.RegularExpressions;

using Chirp.Web.Pages.Shared.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDTO> Cheeps { get; set; } = new();
    private readonly SignInManager<User> _signInManager;
    [BindProperty]
    public CheepBoxModel CheepBox { get; set; } = new();

    public UserTimelineModel(ICheepRepository repository, SignInManager<User> signInManager)
    {
        _repository = repository;
        _signInManager = signInManager;
    }

    public ActionResult OnGet(string user, [FromQuery(Name = "page")] int page = 1)
    {
        page = Math.Max(0, page - 1);
        string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        Regex regex = new(emailPattern);
        //not very beautiful, but im just gonna test this out - feel free to refactor!!
        if(User.Identity.IsAuthenticated && User.Identity.Name == user)
        {
            User? currentUser = _signInManager.UserManager.GetUserAsync(User).Result;
            Cheeps = _repository.GetCheepsFromUserName(currentUser.UserName, page).Result.ToList();
            List<User> following = _repository.GetFollowing(currentUser).Result;
            foreach (User followee in following)
            {
                Cheeps.AddRange(_repository.GetCheepsFromUserName(followee.UserName, page).Result.ToList());
            }
            Cheeps = Cheeps.OrderByDescending(cheep => cheep.TimeStamp).ToList();
            return Page();
        }
        else if (regex.IsMatch(user))
        {
            Cheeps = _repository.GetCheepsFromEmail(user, page).Result.ToList();
        }
        else
        {
            Cheeps = _repository.GetCheepsFromUserName(user, page).Result.ToList();
        }
        return Page();
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
            return RedirectToPage();
        }
        _repository.CreateCheep(user, CheepBox.Message ?? throw new InvalidOperationException("Cheep message is null!")); // we should never get to the exception because of the validation
        TempData["alert-success"] = "Cheep posted successfully!";
        return RedirectToPage();
    }

    public IActionResult OnPostFollow(string followee)
    {
        User? follower = _signInManager.UserManager.GetUserAsync(User).Result;
        User? followeeUser = _repository.GetUserByString(followee).Result;
        _repository.FollowUser(follower, followeeUser);
        TempData["alert-success"] = "User followed successfully!";
        return RedirectToPage();
    }
}
