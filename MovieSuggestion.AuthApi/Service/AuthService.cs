using Microsoft.AspNetCore.Identity;
using MovieSuggestion.AuthApi.Data;
using MovieSuggestion.AuthApi.Models;
using MovieSuggestion.AuthApi.Models.Dto;
using MovieSuggestion.AuthApi.Service.IService;

namespace MovieSuggestion.AuthApi.Service
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTTokenGenerator _jwtTokenGenerator;

        public AuthService(AuthDbContext authDbContext,UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,IJWTTokenGenerator jWTTokenGenerator)
        {
            _db = authDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jWTTokenGenerator;

        }
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = ""
                };
            }
            //if user found , Generate JWT Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Token = token 
            };
            return loginResponseDto;

        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

                if (result.Succeeded) {
                    var userToReturn = _db.ApplicationUsers.First(u=>u.UserName==registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
