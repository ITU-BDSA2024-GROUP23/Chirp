using System.ComponentModel.DataAnnotations;

namespace Chirp.Web.Pages.Shared.Models
{
    public class CheepBoxModel
    {
        [Required]
        [StringLength(160, MinimumLength = 1, ErrorMessage = "Cheep must be between {2} and {1} characters.")]
        [Display(Name = "Message")]
        public string? Message { get; set; }
    }
}
