namespace IS.Controller;

using IS.DTOs;
using IS.Entities;
using IS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



[ApiController]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }


    [Authorize]
    [HttpGet("products")]

    public async Task<ActionResult<List<ReadProductDTO>>> GetProducts()
    {
        var list = await _productService.GetProductsAsync();

        if (!list.Any())
            return NotFound("Products not found");

        return Ok(list);

    }

    [Authorize]
    [HttpGet("{id}")]
 
    public async Task<ActionResult> GetProduct(Guid id)
    {
        var prod = await _productService.GetProductAsync(id);
        if (prod == null)
            return NotFound("No such product");
        return Ok(prod);
    }
    [Authorize(Roles = "admin")]
    [HttpPost("post")]
    public async Task<ActionResult<ReadProductDTO>> PostProduct (PostProductDTO postProductDTO)
    {
        if (postProductDTO == null)
            return BadRequest("PostProductDTO can not be null");

        var post = await _productService.PostProductAsync(postProductDTO);
        return CreatedAtAction(nameof(GetProduct), new { id = post.Id }, post);
    }



    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
       var deleted = await _productService.DeleteProductAsync(id);
        if (!deleted)
            return BadRequest("No such product to delete");
        return NoContent();
    }




}

