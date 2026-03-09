using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Shared.DTOs.Identity
{
    public record LoginDTO([EmailAddress] string Email, string Password);
}
