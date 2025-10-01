namespace IS.Services;
using IS.DbContext;
using IS.DTOs;
using IS.Entities;

public class OrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly AppDbContext _context;


    public OrderService (AppDbContext context, ILogger<OrderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Order> MakeOrderAsync(Guid userId, CreateOrderDTO dto)
    {

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            User = user,
            DateCreated = DateTime.UtcNow,
            TotalAmount = 0
        };

        foreach (var itemDto in dto.Items)
        {
            var product = await _context.Products.FindAsync(itemDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product {itemDto.ProductId} not found");

            if (product.QuantityInStock < itemDto.Quantity)
                throw new InvalidOperationException($"Not enough stock for product {product.Name}");

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                Order = order,
                OrderId = order.Id,
                Product = product,
                ProductId = product.Id,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price
            };

            order.OrderItems.Add(orderItem);

            product.QuantityInStock -= itemDto.Quantity;

            order.TotalAmount += itemDto.Quantity * product.Price;
        }

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        return order;
    }




}
