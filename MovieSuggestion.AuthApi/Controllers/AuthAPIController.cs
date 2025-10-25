using Microsoft.AspNetCore.Mvc;
using MovieSuggestion.AuthApi.Models.Dto;
using MovieSuggestion.AuthApi.Service.IService;

namespace MovieSuggestion.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMsg = await _authService.Register(model);
            if (errorMsg == null) {
                 return BadRequest(errorMsg);
            }

            return Ok("Registration successful!...");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse  = await _authService.Login(model);

            if (loginResponse.User == null)
            {
                return Unauthorized("Invalid Username or Password!...");
            }
            return Ok(loginResponse);
        }
    }
}
