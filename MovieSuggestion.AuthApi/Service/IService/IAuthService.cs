using MovieSuggestion.AuthApi.Models.Dto;

namespace MovieSuggestion.AuthApi.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto>Login(LoginRequestDto loginRequestDto);

    }
}
