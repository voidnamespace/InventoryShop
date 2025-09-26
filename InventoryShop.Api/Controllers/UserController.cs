namespace IS.ApiController;
using IS.DTOs;
using IS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;



    public UserController (UserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var allReadUserDTO = await _userService.GetAllAsync();
        if (allReadUserDTO == null) 
            return NotFound("Users not found");
        return Ok(allReadUserDTO);  
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task <ActionResult> GetById(Guid id)
    {
        var readUserDTO = await _userService.GetById(id);
        return Ok(readUserDTO);
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ReadUserDTO>> Post (PostUserDTO postUserDTO)
    {
        var post = await _userService.PostAsync(postUserDTO);
        return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete (Guid id)
    {
        var delete = await _userService.DeleteAsync(id);
        return NoContent();
    }

}
