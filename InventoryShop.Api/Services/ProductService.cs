namespace IS.Services;
using IS.DbContext;
using IS.Entities;
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


    public async Task<List<Product> GetFullListProducts()
    {
        var list = await _context.Products.ToListAsync();
           
    }






}

