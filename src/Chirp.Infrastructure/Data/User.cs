using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure.Data
{
    public class User : IdentityUser
    {
        public new required string UserName;
    }
}