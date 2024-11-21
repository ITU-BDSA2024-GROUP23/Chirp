using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    [Required]
    public required ICollection<Follower> Following { get; set; }
    [Required]
    public required ICollection<Follower> Followers { get; set; }
    [Required]
    public override required string UserName { get; set; }
    [Required]
    public override required string Email { get; set; }
}
