namespace IS.Controller;
using IS.DTOs;
using IS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
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
        if (!allReadUserDTO.Any())
            return NotFound("Users not found");
        return Ok(allReadUserDTO);  
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task <ActionResult> GetById(Guid id)
    {
        var readUserDTO = await _userService.GetById(id);
        if (readUserDTO == null)
            return NotFound($"User with id {id} not found");
        return Ok(readUserDTO);
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ReadUserDTO>> Post (PostUserDTO postUserDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var post = await _userService.PostAsync(postUserDTO);
        return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
    }
    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put (Guid id, PutUserDTO putUserDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var put = await _userService.PutAsync(id, putUserDTO);
        if (put == null)
            return NotFound($"User with id {id} not found");
        return Ok(put);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete (Guid id)
    {
        var delete = await _userService.DeleteAsync(id);
        if (!delete)
            return NotFound($"User with id {id} not found");
        return NoContent();
    }

}
