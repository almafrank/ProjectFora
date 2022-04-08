using Microsoft.AspNetCore.Identity;

namespace ProjectFora.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Token { get; set; } = string.Empty;
    }
}
