using Microsoft.AspNetCore.Identity;

namespace MovieSuggestion.AuthApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
