using BuildAspnetcoreWebapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuildAspnetcoreWebapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : Controller
{
    private readonly SampleContext _sampleContext;
    public ProductsController(SampleContext sampleContext)
    {
        _sampleContext = sampleContext ?? throw new ArgumentNullException(nameof(sampleContext));
        
        _sampleContext.Database.EnsureCreated();
    }

    [HttpGet("/products")]
    public async Task<ActionResult> GetAllProducts()
    {
        var result = await _sampleContext.Products.ToArrayAsync();

        return Ok(result);
    }

    [HttpGet("/products/{id}")]
    public async Task<ActionResult> GetProduct(int id)
    {
        var result = await _sampleContext.Products.FindAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("/products")]
    public async Task<ActionResult> PostProduct(Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        _sampleContext.Products.Add(product);

        await _sampleContext.SaveChangesAsync();

        return CreatedAtAction("GetProduct", new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutProduct(int id, [FromBody] Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        _sampleContext.Entry(product).State = EntityState.Modified;

        try
        {
            await _sampleContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_sampleContext.Products.Any(p => p.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await _sampleContext.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        _sampleContext.Products.Remove(product);

        await _sampleContext.SaveChangesAsync();

        return product;
    }

    [HttpPost("Delete")]
    public async Task<ActionResult> DeleteMultipleProducts([FromQuery] int[] ids)
    {
        var products = new List<Product>();

        foreach (var id in ids)
        {
            var product = await _sampleContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            products.Add(product);
        }

        _sampleContext.Products.RemoveRange(products);

        await _sampleContext.SaveChangesAsync();

        return Ok(products);
    }
}
