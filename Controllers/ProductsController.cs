using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HPlusSport.Classes;
using HPlusSport.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HPlusSport.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProjectContext _context;
        private readonly ILogger _logger;

        public ProductsController(ProjectContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _context.Database.EnsureCreated();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            IQueryable<Product> products = _context.Products;

            if (queryParameters.MinPrice != null && queryParameters.MaxPrice != null)
            {
                products = products
                .Where(p =>
                p.Price >= queryParameters.MinPrice.Value &&
                p.Price <= queryParameters.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(queryParameters.Sku))
                products = products.Where(p => p.Sku == queryParameters.Sku);


            if (!string.IsNullOrEmpty(queryParameters.Name))
                products = products.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));


            products = products
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Take(queryParameters.Size);

            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                products = products.OrderBy(c => c.Price);
            }

            return Ok(await products.Include(c => c.Category).ToArrayAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            _logger.LogInformation("Getting Product: {prod}", JsonConvert.SerializeObject(product));

            return Ok(await product);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = product.Id },
                product
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Products.Find(id) == null)
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

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Product>> DeleteProduct([FromRoute] int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return product;
        }
    }
}