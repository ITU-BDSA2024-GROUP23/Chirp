using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public UserDTO? ToUserDTO()
    {
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
