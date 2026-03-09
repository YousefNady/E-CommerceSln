using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            return User != null;
        }

        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);

            if (User is null)
                return Error.NotFound("User.NotFound", $"No User With Email {email} was Found");

            return new UserDTO(User.Email, User.DisplayName, await CreateTokenAsync(User));
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (User is null)
            {
                return Error.InvalidCredentials("User.InvalidCredentials");
            }

            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, loginDTO.Password);
            if (!IsPasswordValid)
            {
                return Error.InvalidCredentials("User.InvalidCredentials");
            }

            var Token = await CreateTokenAsync(User);
            return new UserDTO(User.Email!, User.DisplayName, Token);
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser()
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName
            };

            var IdentityResult = await _userManager.CreateAsync(User, registerDTO.Password);

            if (IdentityResult.Succeeded)
            {
                var Token = await CreateTokenAsync(User);
                return new UserDTO(User.Email, User.DisplayName, Token);
            }

            return IdentityResult.Errors.Select(E => Error.Validation(E.Code, E.Description)).ToList();
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            // Token [Issuer , Audience , Claims , Expires , SigningCredentials]

            var Claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                };

            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var SecretKey = _configuration["JWTOptions:SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                claims: Claims,
                signingCredentials: Cred);

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
