using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    [Required]
    public required ICollection<Follower> Following { get; set; }
    [Required]
    public required ICollection<Follower> Followers { get; set; }

    public UserDTO? ToUserDTO() {
        if (UserName == null) 
        {
            return null;
        }
        if (Email == null) 
        {
            return null;
        }

        return new UserDTO(
            Id: Id,
            UserName: UserName,
            Email: Email
        );
    }
}
