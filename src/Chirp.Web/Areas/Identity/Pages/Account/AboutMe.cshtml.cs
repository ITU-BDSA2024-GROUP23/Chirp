using System.IO.Compression;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


public class AboutMeModel : PageModel
{
    private readonly SignInManager<User> _signInManager;
    private readonly IUserService _userService;
    private readonly ICheepService _cheepService;

    public UserInfoDTO? UserInfo { get; set; }

    public AboutMeModel(
        SignInManager<User> signInManager,
        IUserService userService,
        ICheepService cheepService)
    {
        _signInManager = signInManager;
        _userService = userService;
        _cheepService = cheepService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        User? currentUser = await _signInManager.UserManager.GetUserAsync(User);
        if (currentUser == null)
        {
            TempData["alert-error"] = "You are not logged in.";
            return RedirectToPage("/Account/Login");
        }

        string? userName = currentUser.UserName;
        if (userName == null)
        {
            TempData["alert-error"] = "You are not logged in.";
            return RedirectToPage("/Account/Login");
        }

        UserInfo = await _userService.GetUserInfoDTO(userName);
        if (UserInfo == null)
        {
            TempData["alert-error"] = "An error occured. Please retry.";
            return RedirectToPage("/Account/Login");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteMeAsync()
    {
        User? currentUser = await _signInManager.UserManager.GetUserAsync(User);
        if (currentUser == null)
        {
            TempData["alert-error"] = "You are not logged in.";
            return RedirectToPage("/Account/Login");
        }

        var success = await _userService.DeleteUser(currentUser.ToUserDTO());
        if (!success)
        {
            TempData["alert-error"] = "An error occured. Please retry.";
            return RedirectToPage();
        }

        await _signInManager.SignOutAsync();
        return RedirectToPage("/Account/Login");
    }

    public async Task<IActionResult> OnPostGetInfoAsync()
    {
        var currentUser = await _signInManager.UserManager.GetUserAsync(User);
        if (UserInfo == null)
        {
            TempData["alert-error"] = "An error occured. Please retry.";
            return Page();
        }


        return Page();
    }

    private string GenerateFollowingCSV(UserDTO user)
    {
        var following = _userService.GetFollowing(user).Result;
        var csv = new StringBuilder();
        csv.AppendLine("Following " + user.UserName);
        foreach (var followee in following)
        {
            csv.AppendLine(followee.UserName);
        }
        return csv.ToString();
    }

    private string GenerateFollowersCSV(UserDTO user)
    {
        var followers = _userService.GetFollowers(user).Result;
        var csv = new StringBuilder();
        csv.AppendLine("Followers to " + user.UserName);
        foreach (var follower in followers)
        {
            csv.AppendLine(follower.UserName);
        }
        return csv.ToString();
    }

    private string GenerateCheepsCSV(UserDTO user)
    {
        var cheeps = _cheepService.GetCheepsFromUserName(user.UserName).Result;
        var csv = new StringBuilder();
        csv.AppendLine("Author, Cheep, TimeStamp");
        foreach (var cheep in cheeps)
        {
            csv.AppendLine($"{cheep.Author}: {cheep.Text} ({cheep.TimeStamp})");
        }
        return csv.ToString();
    }

    private string GenerateUserCSV(UserDTO user)
    {
        var csv = new StringBuilder();
        csv.AppendLine($"Username: {user.UserName}");
        csv.AppendLine($"Email: {user.Email}");
        return csv.ToString();
    }

    private void AddCSVToZip(ZipArchive archive, string csv, string fileName)
    {
        var entry = archive.CreateEntry(fileName);
        using (var writer = new StreamWriter(entry.Open()))
        {
            writer.Write(csv);
        }
    }

    public async Task<IActionResult> OnPostDownloadInfoAsync()
    {
        var currentUser = await _signInManager.UserManager.GetUserAsync(User);
        if (currentUser == null)
        {
            TempData["alert-error"] = "You are not logged in.";
            return RedirectToPage("/Account/Login");
        }
        var userDTO = currentUser.ToUserDTO();
        if (userDTO == null)
        {
            TempData["alert-error"] = "You are not logged in.";
            return RedirectToPage("/Account/Login");
        }

        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            AddCSVToZip(archive, GenerateUserCSV(userDTO), "userinfo.csv");
            AddCSVToZip(archive, GenerateCheepsCSV(userDTO), "cheeps.csv");
            AddCSVToZip(archive, GenerateFollowingCSV(userDTO), "following.csv");
            AddCSVToZip(archive, GenerateFollowersCSV(userDTO), "followers.csv");
        }
        return File(memoryStream.ToArray(), "application/zip", "info.zip");
    }
}
