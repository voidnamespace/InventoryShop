namespace IS.PasswordService;
using IS.Entities;
using Microsoft.AspNetCore.Identity;

public class PasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher;


    public PasswordService()
    {
        _passwordHasher = new PasswordHasher<User>();
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }
    public bool VerifyPassword(User user, string hashedPassword, string providedPassword) 
    {
        var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }

}