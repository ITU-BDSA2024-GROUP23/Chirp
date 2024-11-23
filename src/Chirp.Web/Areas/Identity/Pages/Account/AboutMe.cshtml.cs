using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


public class AboutMeModel : PageModel
{
    private readonly SignInManager<User> _signInManager;
    private readonly ICheepRepository _repository;

    public UserInfoDTO? UserInfo { get; set; }

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
            return RedirectToPage("/Account/Login");
        }

        await PrepareInfo(currentUser);
        
        return Page();
    }

    private async Task PrepareInfo(User currentUser) 
    {
        UserInfo = new UserInfoDTO
        {
            UserName = currentUser.UserName,
            Email = currentUser.Email,
            Cheeps = await _repository.GetCheepsFromUserName(currentUser.UserName),
            Following = await _repository.GetFollowing(currentUser),
            Followers = await _repository.GetFollowers(currentUser)
        };
        
    }

    public async Task<IActionResult> OnPostForgetMeAsync()
    {
        var currentUser = await _signInManager.UserManager.GetUserAsync(User);
        if (currentUser == null)
        {
            TempData["alert-error"] = "You are not logged in.";
            return RedirectToPage("/Account/Login");
        }
        await _repository.ForgetMe(currentUser);
        await _signInManager.SignOutAsync();
        return RedirectToPage("/Account/Login");
    }
}
