namespace IS.DTOs;
using Microsoft.AspNetCore.Identity;
using IS.Enums;
using System.ComponentModel.DataAnnotations;

public class PostUserDTO
{
    [Required]
    [MaxLength(25)]
    public string UserName { get; set; } = String.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; } = String.Empty;

    [Required]
    public string Password {  get; set; } = String.Empty;
}