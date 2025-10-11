namespace IS.Services;
using IS.DTOs;
using Microsoft.AspNetCore.Mvc;
using IS.DbContext;
using IS.Entities;
using Microsoft.EntityFrameworkCore;

public class UserService 
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserService> _logger;
    private readonly PasswordService _passwordService;
    

    public UserService (AppDbContext context, ILogger<UserService> logger, PasswordService passwordService)
    {
        _context = context;
        _logger = logger;
       _passwordService = passwordService;
    }

    
    public async Task<List<ReadUserDTO>> GetAllAsync()
    {
        _logger.LogInformation("Request to get all users");

        var users = await _context.Users.ToListAsync();

        if (!users.Any())
            _logger.LogInformation("Users not found");

        var usersToDTO = users.Select(x => new ReadUserDTO
        {
            Id = x.Id,
            UserName = x.UserName,
            Email = x.Email
        }).ToList();

        return usersToDTO;

    }

    public async Task <ReadUserDTO?> GetById (Guid id)
    {
        _logger.LogInformation("Request to get user with {id}", id);

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            _logger.LogWarning("User with id {id} not found ", id);
            return null;
        }
          

        var readUserDTO = new ReadUserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };

        return readUserDTO;

    }

    public async Task <ReadUserDTO> PostAsync (PostUserDTO postUserDTO)
    {
        if (postUserDTO == null)
            throw new ArgumentNullException("postUserDTO can not be null");

        _logger.LogInformation("Request to post new user");

        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == postUserDTO.Email);
        

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = postUserDTO.UserName,
            Email = postUserDTO.Email,   
            Role = Enums.Roles.Customer
        };

        user.PasswordHash = _passwordService.HashPassword(user, postUserDTO.Password);

        await _context.AddAsync(user);

        await _context.SaveChangesAsync();

        _logger.LogInformation("User created with id {userID}", user.Id);

        var readUserDTO = new ReadUserDTO
        {
           Id = user.Id, 
           UserName = user.UserName,
           Email = user.Email, 
        };
        return readUserDTO;
    }

    public async Task<ReadUserDTO?> PutAsync(Guid id, PutUserDTO putUserDTO)
    {
  
        if (putUserDTO == null)
            throw new ArgumentNullException("putUserDTO can not be null");

        var currentUser = await _context.Users.FindAsync(id);
        if (currentUser == null)
            return null;

        currentUser.UserName = putUserDTO.UserName ?? currentUser.UserName;
        currentUser.Email = putUserDTO.Email ?? currentUser.Email;


        if (!string.IsNullOrEmpty(putUserDTO.Password))
        {
            currentUser.PasswordHash = _passwordService.HashPassword(currentUser, putUserDTO.Password);
        }


        await _context.SaveChangesAsync();

        var readUserDTO = new ReadUserDTO
        {
            Id = currentUser.Id,
            UserName = currentUser.UserName,
            Email = currentUser.Email,
        };
        return readUserDTO;

    }


    public async Task<bool> DeleteAsync (Guid id)
    {
        var delUser = await _context.Users.FindAsync(id);

        if (delUser == null)
        {
            _logger.LogWarning("Request to delete non-existing user with id {userId}", id);
            return false;
        }

        _logger.LogInformation("Request to delete user with id {userId}", id);

        _context.Users.Remove(delUser);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User with id {userId} has been deleted", id);
        return true;
    }

    public async Task<ReadUserDTO?> RegisterAsync (PostUserDTO postUserDTO)
    {
        if (postUserDTO == null)
            throw new ArgumentNullException(nameof(postUserDTO));


        _logger.LogInformation("Request to register new user with email {Email}", postUserDTO.Email);

        bool emailExists = await _context.Users.AnyAsync(u => u.Email == postUserDTO.Email);
        if (emailExists)
        {
            throw new ArgumentException("User with this email already exists");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = postUserDTO.UserName,
            Email = postUserDTO.Email
        };

        user.PasswordHash = _passwordService.HashPassword(user, postUserDTO.Password);

        await _context.AddAsync(user);

        await _context.SaveChangesAsync();

        _logger.LogInformation("User created with id {userID}", user.Id);

        var readUserDTO = new ReadUserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
        };
        return readUserDTO;
    }

}