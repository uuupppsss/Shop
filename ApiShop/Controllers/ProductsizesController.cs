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
using Microsoft.AspNetCore.SignalR.Client;

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
                    Id = s.Id,
                    ProductId = s.ProductId,
                    Size = s.Size,
                    Count = s.Count
                });
            }
            return Ok(result);
        }

        // PUT: api/Productsizes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPut("{product_id}")]
        public async Task<ActionResult> PutProductsize(int product_id, List<ProductSizeDTO> productsize)
        {
            var newSizes = productsize.Where(s => s.Id == 0).ToList();
            foreach (var s in newSizes)
            {
                _context.Productsizes.Add(new Productsize
                {
                    Size = s.Size,
                    Count = s.Count,
                    ProductId = product_id
                });
            
            }

            try
            {
                await _context.SaveChangesAsync();
                HubConnection connection = new HubConnectionBuilder()
                       .WithUrl("http://localhost:5226/clientshub").Build();
                await connection.StartAsync();
                await connection.SendAsync("ProductSizesUpdated", product_id);
                await connection.StopAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }

            var updatedSizes = productsize.Where(s => s.ProductId == product_id).ToList();
            foreach (var s in updatedSizes)
            {
                var found_size = await _context.Productsizes
                    .FirstOrDefaultAsync(s1 => s1.Id == s.Id);
                if (found_size != null)
                {
                    found_size.Size = s.Size;
                    found_size.Count = s.Count;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // POST: api/Productsizes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPost("{product_id}")]
        public async Task<ActionResult> PostProductsize(int product_id, List<ProductSizeDTO> sizes)
        {
            foreach (var s in sizes)
            {
                _context.Productsizes.Add(new Productsize
                {
                    Size = s.Size,
                    ProductId = product_id,
                    Count = s.Count
                });
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
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


    }
}
