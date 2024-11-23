using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


public class AboutMeModel : PageModel
{
    private readonly SignInManager<User> _signInManager;
    private readonly ICheepRepository _repository;

    public UserDTO? UserInfo { get; set; }

    public IList<CheepDTO> RecentCheeps { get; set; } = new List<CheepDTO>();

    public AboutMeModel(SignInManager<User> signInManager, ICheepRepository repository)
    {
        _signInManager = signInManager;
        _repository = repository;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        // Get the logged-in user
        var currentUser = await _signInManager.UserManager.GetUserAsync(User);
        if (currentUser == null)
        {
            TempData["alert-error"] = "You are not logged in.";
            return RedirectToPage("/");
        }

        // Populate user information
        UserInfo = new UserDTO
        {
            UserName = currentUser.UserName,
            FollowersCount = (await _repository.GetFollowers(currentUser)).Count,
            FollowingCount = (await _repository.GetFollowing(currentUser)).Count
        };

        // Optionally, fetch recent activity (e.g., recent cheeps)
        RecentCheeps = await _repository.GetCheepsFromUserName(currentUser.UserName, page: 0);
        return Page();
    }
}
