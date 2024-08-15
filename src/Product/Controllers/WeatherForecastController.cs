using Microsoft.AspNetCore.Mvc;

namespace Product.Controllers;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    static List<Product> Products = new List<Product>()
        {
            new Product { Id = 1, Name = "Product 1", Price = 10.0m },
            new Product { Id = 2, Name = "Product 2", Price = 20.0m },
            new Product { Id = 3, Name = "Product 3", Price = 30.0m }
        };
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> Get()
    {
        var products = await GetProductsAsync();
        await Task.Delay(500);
        return Ok(products);
    }


    [HttpPost]
    public async Task<ActionResult<Product>> Post(Product product)
    {
        Products.Add(product);
        await Task.Delay(500);
        // Return the product along with a 201 Created status code
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    private Task<List<Product>> GetProductsAsync()
    {

        return Task.FromResult(Products);
    }
}