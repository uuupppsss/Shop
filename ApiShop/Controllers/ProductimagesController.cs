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
    public class ProductimagesController : ControllerBase
    {
        private readonly ShopdbContext _context;

        public ProductimagesController(ShopdbContext context)
        {
            _context = context;
        }

        // GET: api/Productimages
        [HttpGet("{product_id}")]
        public async Task<ActionResult<List<ProductImageDTO>>> GetProductimages(int product_id)
        {
            List<ProductImageDTO> result = new();
            foreach(var item in _context.Productimages.Where(i=>i.ProductId==product_id))
            {
                result.Add(new ProductImageDTO
                {
                    Id=item.Id,
                    ProductId=item.ProductId,
                    Image=item.Image,
                });
            }
            return Ok(result);
        }

        // GET: api/Productimages/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Productimage>> GetProductimage(int id)
        //{
        //    var productimage = await _context.Productimages.FindAsync(id);

        //    if (productimage == null)
        //    {
        //        return NotFound();
        //    }

        //    return productimage;
        //}

        // PUT: api/Productimages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProductimage(int id, Productimage productimage)
        //{
        //    if (id != productimage.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(productimage).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductimageExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Productimages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [Authorize(Roles = "admin")]
        [HttpPost("{product_id}")]
        public async Task<ActionResult> PostProductimages(int product_id, List<byte[]> sent_images)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == product_id);
            if (product == null) return NotFound();
            foreach (var sent_image in sent_images)
            {
                _context.Productimages.Add(new Productimage
                {
                    ProductId=product_id,
                    Image=sent_image,
                    Product=product
                });
            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Productimages/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductimage(int id)
        {
            var productimage = await _context.Productimages.FindAsync(id);
            if (productimage == null)
            {
                return NotFound();
            }

            _context.Productimages.Remove(productimage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //private bool ProductimageExists(int id)
        //{
        //    return _context.Productimages.Any(e => e.Id == id);
        //}
    }
}
