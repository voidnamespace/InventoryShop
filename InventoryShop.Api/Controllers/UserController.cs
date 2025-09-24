namespace IS.ApiController;
using IS.DTOs;
using IS.UserService;
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


    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var allReadUserDTO = await _userService.GetAllAsync();
        if (allReadUserDTO == null) 
            return NotFound("Users not found");
        return Ok(allReadUserDTO);  
    }



    [HttpGet("{id}")]
    public async Task <ActionResult> GetById(Guid id)
    {
        var readUserDTO = await _userService.GetById(id);
        return Ok(readUserDTO);
    }


    [HttpPost]
    public async Task<ActionResult<ReadUserDTO>> Post (PostUserDTO postUserDTO)
    {
        var post = await _userService.PostAsync(postUserDTO);
        return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);

    }

    [HttpDelete]
    public async Task<IActionResult> Delete (Guid id)
    {
        var delete = await _userService.DeleteAsync(id);
        return NoContent();
    }





}





