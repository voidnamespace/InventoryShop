namespace IS.Services;
using IS.DbContext;
using IS.DTOs;
using IS.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class OrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly AppDbContext _context;
    public OrderService (AppDbContext context, ILogger<OrderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ReadOrderDTO> MakeOrderAsync(Guid userId, CreateOrderDTO createOrderDTO)
    {
        _logger.LogInformation("Request to create order user {userId}", userId);

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

        foreach (var itemDto in createOrderDTO.Items)
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

            _logger.LogInformation("Product {productId} stock decreased by {quantity}", product.Id, itemDto.Quantity);

        }

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Order {orderId} created for user {userId}", order.Id, userId);

        var readOrderDTO = new ReadOrderDTO
        {
            Id = order.Id,
            DateCreated = order.DateCreated,
            TotalAmount = order.TotalAmount,
            UserId = user.Id,
            UserName = user.UserName,
            Items = order.OrderItems.Select(oi => new ReadOrderItemDTO
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice

            }).ToList()
        };

        return readOrderDTO;
    }
    public async Task<List<ReadOrderDTO>> GetAllOrdersAsCustomerAsync(Guid userId)
    {

        _logger.LogInformation("Request to get all own orders of user {userId}", userId);
        
        var user = await _context.Users
            .Include(u => u.Orders)
                .ThenInclude(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException($"No such user {userId}");

        if (!user.Orders.Any())
            return new List<ReadOrderDTO>();


        var readOrders = user.Orders.Select(order => new ReadOrderDTO
        {
            Id = order.Id,
            DateCreated = order.DateCreated,
            TotalAmount = order.TotalAmount,
            UserId = user.Id,
            UserName = user.UserName,
            Items = order.OrderItems.Select(oi => new ReadOrderItemDTO
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
            }).ToList()
        }).ToList();

        return readOrders;
    }

    public async Task<ReadOrderDTO> GetOrderByIdAsCustomerAsync(Guid userId, Guid orderId)
    {
        _logger.LogInformation("Request to get order {orderId} for user {userId}", orderId, userId);
        
        var user = await _context.Users
            .Include(x => x.Orders)
            .ThenInclude(y => y.OrderItems)
            .ThenInclude(z => z.Product)
            .FirstOrDefaultAsync(p => p.Id == userId);

        if (user == null)
        {
            _logger.LogWarning("User with id {id} not found", userId);
            throw new KeyNotFoundException($"User {userId} not found");
        }
            
        var neededOrder = user.Orders.FirstOrDefault(x => x.Id == orderId);
        if (neededOrder == null)
        {
            _logger.LogWarning("Order {orderId} for user {userId} not found", orderId, userId);
            throw new KeyNotFoundException($"Order {orderId} not found for user {userId}");
        }
            

        var readOrderDTO = new ReadOrderDTO
        {
            Id = neededOrder.Id,
            DateCreated = neededOrder.DateCreated,
            TotalAmount = neededOrder.TotalAmount,
            UserId = user.Id,
            UserName = user.UserName,
            Items = neededOrder.OrderItems.Select(oi => new ReadOrderItemDTO
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,

            }).ToList()
        };
        _logger.LogInformation("Successfully retrieved order {orderId} for user {userId}", orderId, userId);

        return readOrderDTO;

    }
    
    public async Task<List<ReadOrderDTO>> GetAllOrdersAsAdminAsync(Guid userId)
    {
        var user = await _context.Users
            .Include(c => c.Orders)
            .ThenInclude(x => x.OrderItems)
            .ThenInclude(y => y.Product)
        .FirstOrDefaultAsync(z => z.Id == userId);

        if (user == null)
            throw new KeyNotFoundException($"No such user {userId}");

        if (!user.Orders.Any())
            return new List<ReadOrderDTO>();

        var readOrders = user.Orders.Select(order => new ReadOrderDTO
        {
            Id = order.Id,
            DateCreated = order.DateCreated,
            TotalAmount = order.TotalAmount,
            UserId = user.Id,
            UserName = user.UserName,
            Items = order.OrderItems.Select(oi => new ReadOrderItemDTO
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
            }).ToList()
        }).ToList();

        return readOrders;
    }
}
