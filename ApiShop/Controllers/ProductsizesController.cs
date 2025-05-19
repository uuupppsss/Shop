using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiShop.Model;
using ShopLib;
using Microsoft.AspNetCore.Authorization;

namespace ApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsizesController : ControllerBase
    {
        private readonly ShopdbContext _context;

        public ProductsizesController(ShopdbContext context)
        {
            _context = context;
        }

        // GET: api/Productsizes
        [HttpGet("{product_id}")]
        public async Task<ActionResult<List<ProductSizeDTO>>> GetProductsizes(int product_id)
        {
            List<ProductSizeDTO> result = new();
            foreach(var s in _context.Productsizes.Where(s=>s.ProductId==product_id&& s.Count>0))
            {
                result.Add(new ProductSizeDTO
                {
                    Id = s.ProductId,
                    ProductId = s.ProductId,
                    Size = s.Size,
                    Count = s.Count
                });
            }
            return Ok(result);
        }

        // GET: api/Productsizes/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Productsize>> GetProductsize(int id)
        //{
        //    var productsize = await _context.Productsizes.FindAsync(id);

        //    if (productsize == null)
        //    {
        //        return NotFound();
        //    }

        //    return productsize;
        //}

        // PUT: api/Productsizes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductsize(int id, Productsize productsize)
        {
            if (id != productsize.Id)
            {
                return BadRequest();
            }

            _context.Entry(productsize).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsizeExists(id))
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

        // POST: api/Productsizes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Productsize>> PostProductsize(Productsize productsize)
        {
            _context.Productsizes.Add(productsize);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductsize", new { id = productsize.Id }, productsize);
        }

        // DELETE: api/Productsizes/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductsize(int id)
        {
            var productsize = await _context.Productsizes.FindAsync(id);
            if (productsize == null)
            {
                return NotFound();
            }

            _context.Productsizes.Remove(productsize);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductsizeExists(int id)
        {
            return _context.Productsizes.Any(e => e.Id == id);
        }
    }
}
