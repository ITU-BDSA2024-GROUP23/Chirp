using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDTO> Cheeps { get; set; }

    public UserTimelineModel(ICheepRepository repository)
    {
        _repository = repository;
        Cheeps = new();
    }

    public ActionResult OnGet(string user, [FromQuery(Name = "page")] int page = 1)
    {
        page = Math.Max(0, page-1);
        string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        Regex regex = new(emailPattern);
        if(regex.IsMatch(user))
        {
            Cheeps = _repository.GetCheepsFromEmail(user, page).Result.ToList();
        } 
        else 
        {
            Cheeps = _repository.GetCheepsFromName(user, page).Result.ToList();
        }
        return Page();
    }
}
