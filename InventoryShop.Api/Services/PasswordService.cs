using Microsoft.AspNetCore.Identity;
using IS.Entities;
using Microsoft.AspNetCore.Mvc;
using IS.DbContext;
public class PasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AppDbContext _context;
    
    
    public PasswordService (IPasswordHasher<User> passwordHasher, AppDbContext context)
    {
        _passwordHasher = passwordHasher;
        _context = context;
    }



    public string HashPassword(User user, string password)
    {
       return  _passwordHasher.HashPassword(user, password);
        
    }

    public async Task<bool> VerifyPassWord (User user, string hashedPassword, string providedPassword)
    {        
        var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, providedPassword);
            await _context.SaveChangesAsync();
            return true;
            
        }
        if (result == PasswordVerificationResult.Failed)
            return false;
        if (result == PasswordVerificationResult.Success)
            return true;

        return false;
    }



    public bool VerifyPasswordAndUpdateHash(User user, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, providedPassword);

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, providedPassword);
            _context.SaveChangesAsync();
        }
        bool passsingSuccess = result != PasswordVerificationResult.Failed;
        return passsingSuccess;
    }


}