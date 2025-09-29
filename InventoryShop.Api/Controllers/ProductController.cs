namespace IS.Controller;

using IS.Services;
using Microsoft.AspNetCore.Mvc;



[ApiController]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController (ProductService productService)
    {
        _productService = productService;
    }








}

