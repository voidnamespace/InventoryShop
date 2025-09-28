using System.ComponentModel.DataAnnotations;

namespace IS.DTOs;

public class LoginDTO
{
    [Required]
    public string Password { get; set; } = String.Empty;
    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; } = String.Empty;
}
