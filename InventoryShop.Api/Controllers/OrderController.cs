namespace IS.Controllers;
using IS.DTOs;
using IS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;
    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }






    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ReadOrderDTO>> MakeOrder([FromBody] CreateOrderDTO createOrderDTO)
    {
        var userIdClaim = User.FindFirst("id")?.Value;
        if (userIdClaim == null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);


        var order = await _orderService.MakeOrderAsync(userId, createOrderDTO);
        return Ok(order);
    }


    [Authorize]
    [HttpGet("my-orders")]
    public async Task<ActionResult<List<ReadOrderDTO>>> GetAllOrdersAsCustomer()
    {
        var userIdClaim = User.FindFirst("id")?.Value;
        if (userIdClaim == null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);

        var orders = await _orderService.GetAllOrdersAsCustomerAsync(userId);
        return Ok(orders);
    }




    [Authorize(Roles = "Admin")]
    [HttpGet("user/{userId}/orders")]
    public async Task<ActionResult<List<ReadOrderDTO>>> GetAllOrdersAsAdmin(Guid userId)
    {
        var orders = await _orderService.GetAllOrdersAsAdminAsync(userId);
        return Ok(orders);
    }




}
