using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HPlusSport.Classes;
using HPlusSport.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPlusSport.Controllers
{   
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ProductsController(ProjectContext context)
        {
            _context = context;

            _context.Database.EnsureCreated();
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
            return Ok(await product);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product
            );
        }
    }
}