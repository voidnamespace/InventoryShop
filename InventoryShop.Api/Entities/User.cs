namespace IS.Entities;
using Microsoft.AspNetCore.Identity;
using IS.Enums;
using System.ComponentModel.DataAnnotations;

public class User
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(25)]
    public string UserName { get; set; } = String.Empty;
    [Required]
    [EmailAddress]
    [MaxLength (50)]
    public string Email { get; set; } = String.Empty;
    [Required]
    public string PasswordHash { get; set; } = String.Empty;
    [Required]
    public Roles Role { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();

}