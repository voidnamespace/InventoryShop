namespace IS.Services;
using IS.DTOs;
using IS.DbContext;
using IS.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class ProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly AppDbContext _context;

    public ProductService(ILogger<ProductService> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;

    }
    public async Task<List<ReadProductDTO>> GetProductsAsync()
    {
        _logger.LogInformation("Request to get all products");
        
        var products = await _context.Products.ToListAsync();

        if (!products.Any())
            _logger.LogInformation("Products not found");

        var readProductsDTO = products.Select(x => new ReadProductDTO
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            QuantityInStock = x.QuantityInStock,
        }).ToList();
        
        return readProductsDTO;

    }

    public async Task<ReadProductDTO?> GetProductAsync(Guid id)
    {
        _logger.LogInformation("Request to get product with Id {id}", id);

        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            _logger.LogWarning("Product with id {id} not found", id);
            return null;
        }

        var readProductDTO = new ReadProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            QuantityInStock = product.QuantityInStock,

        };
        return readProductDTO;
    }

    public async Task<ReadProductDTO> PostProductAsync(PostProductDTO postProductDTO)
    {
        if (postProductDTO == null)
            throw new ArgumentNullException("postProductDTO cannot be null");
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = postProductDTO.Name,
            Description = postProductDTO.Description,
            Price = postProductDTO.Price,
            QuantityInStock = postProductDTO.QuantityInStock
        };
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        var readProductDTO = new ReadProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            QuantityInStock = product.QuantityInStock
        };

        return readProductDTO;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var delProd = await _context.Products.FindAsync(id);

        if (delProd == null)
        {
            _logger.LogWarning("Product with id {id} not found", id);
            return false;
        }         

        _logger.LogInformation("Request to delete product with Id {id}", id);

        _context.Products.Remove(delProd);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Product with Id {id} deleted successfully", id);

        return true;
    }

}
