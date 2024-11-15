﻿using System.Text.RegularExpressions;

using Chirp.Web.Pages.Shared.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDTO> Cheeps { get; set; } = new();
    public List<User> Following { get; set; } = new();
    private readonly SignInManager<User> _signInManager;
    [BindProperty]
    public CheepBoxModel CheepBox { get; set; } = new();

    public UserTimelineModel(ICheepRepository repository, SignInManager<User> signInManager)
    {
        _repository = repository;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnGetAsync(string user, [FromQuery(Name = "page")] int page = 1)
    {
        page = Math.Max(0, page - 1);
        string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        Regex regex = new(emailPattern);

        if(User.Identity.IsAuthenticated && User.Identity.Name == user)
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
        User currentUser = await _signInManager.UserManager.GetUserAsync(User);
        Cheeps = await _repository.GetCheepsFromUserName(currentUser.UserName, page);
        Following = await _repository.GetFollowing(currentUser);
        foreach (User followee in Following)
        {
            Cheeps.AddRange(await _repository.GetCheepsFromUserName(followee.UserName, page));
        }
        Cheeps = Cheeps.OrderByDescending(cheep => cheep.TimeStamp).ToList();
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

    public IActionResult OnPostUnfollow(string followee)
    {
        User? follower = _signInManager.UserManager.GetUserAsync(User).Result;
        User? followeeUser = _repository.GetUserByString(followee).Result;
        _repository.UnfollowUser(follower, followeeUser);
        TempData["alert-success"] = "User unfollowed successfully!";
        return RedirectToPage();
    }
}
