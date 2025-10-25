using MovieSuggestion.AuthApi.Models;

namespace MovieSuggestion.AuthApi.Service.IService
{
    public interface IJWTTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
