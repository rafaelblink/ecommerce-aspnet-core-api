using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HPlusSport.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPlusSport.Controllers
{
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
        public async Task<IActionResult> GetAllProducts()
        {
            var products = _context.Products.Include(p => p.Category).ToArrayAsync();
            return Ok(await products);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id) {
            var product = _context.Products.FindAsync(id);
            if(product == null) return NotFound(); 
            return Ok(await product);
        }
    }
}