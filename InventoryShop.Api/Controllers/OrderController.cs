namespace IS.Controllers;
using IS.DTOs;
using IS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;
    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }






    [Authorize]
    [HttpPost("order")]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderDTO dto)
    {
        var userId = Guid.Parse(User.FindFirst("id")!.Value);

        var order = await _orderService.MakeOrderAsync(userId, dto);
        return Ok(order);
    }






}
