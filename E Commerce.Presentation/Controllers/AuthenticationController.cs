using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Presentation.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        // Login
        // POST : baseUrl/api/Authentication/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var Result = await _authenticationService.LoginAsync(loginDTO);
            return HandleResult(Result);
        }

        // Register
        // POST : BaseUrl/api/Authentication/Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var Result = await _authenticationService.RegisterAsync(registerDTO);
            return HandleResult(Result);
        }


        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmail(string Email)
        {
            var Result = await _authenticationService.CheckEmailAsync(Email);
            return Ok(Result);
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email)!;
            var Result = await _authenticationService.GetUserByEmailAsync(Email);
            return HandleResult(Result);
        }
    }
}
