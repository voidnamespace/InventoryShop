namespace IS.Entities;
using Microsoft.AspNetCore.Identity;
using IS.Enums;


public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string PasswordHash { get; set; } = String.Empty;
    public Roles Role { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();

}