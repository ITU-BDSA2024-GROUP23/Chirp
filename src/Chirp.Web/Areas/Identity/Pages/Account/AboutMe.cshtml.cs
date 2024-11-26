using System.IO.Compression;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


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

    public async Task<IActionResult> OnPostDeleteMeAsync()
    {
        var currentUser = await _signInManager.UserManager.GetUserAsync(User);
        if (currentUser == null)
        {
            TempData["alert-error"] = "You are not logged in.";
            return RedirectToPage("/Account/Login");
        }
        await _repository.DeleteUser(currentUser);
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

    private string GenerateFollowingCSV(ZipArchive archive, User user)
    {
        var following = _repository.GetFollowing(user).Result;
        var csv = new StringBuilder();
        csv.AppendLine("Following " + user.UserName);
        foreach (var followee in following)
        {
            csv.AppendLine(followee.UserName);
        }
        return csv.ToString();
    }

    private string GenerateFollowersCSV(ZipArchive archive, User user)
    {
        var followers = _repository.GetFollowers(user).Result;
        var csv = new StringBuilder();
        csv.AppendLine("Followers to " + user.UserName);
        foreach (var follower in followers)
        {
            csv.AppendLine(follower.UserName);
        }
        return csv.ToString();
    }

    private string GenerateCheepsCSV(ZipArchive archive, User user)
    {
        var cheeps = _repository.GetCheepsFromUserName(user.UserName).Result;
        var csv = new StringBuilder();
        csv.AppendLine("Author, Cheep, TimeStamp");
        foreach (var cheep in cheeps)
        {
            csv.AppendLine($"{cheep.Author}: {cheep.Text} ({cheep.TimeStamp})");
        }
        return csv.ToString();
    }

    private string GenerateUserCSV(ZipArchive archive, User user)
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

        using (var memoryStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                AddCSVToZip(archive, GenerateUserCSV(archive, currentUser), "userinfo.csv");
                AddCSVToZip(archive, GenerateCheepsCSV(archive, currentUser), "cheeps.csv");
                AddCSVToZip(archive, GenerateFollowingCSV(archive, currentUser), "following.csv");
                AddCSVToZip(archive, GenerateFollowersCSV(archive, currentUser), "followers.csv");
            }
            return File(memoryStream.ToArray(), "application/zip", "info.zip");
        }
    }
}
