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
    public class ProducttypesController : ControllerBase
    {
        private readonly ShopdbContext _context;

        public ProducttypesController(ShopdbContext context)
        {
            _context = context;
        }

        // GET: api/Producttypes
        [HttpGet]
        public async Task<ActionResult<List<ProductTypeDTO>>> GetProducttypes()
        {
            List<ProductTypeDTO> result = new();
            foreach (var item in _context.Producttypes) 
            {
                result.Add(new ProductTypeDTO
                {
                    Id=item.Id,
                    Title=item.Title,
                });
            }
            return Ok(result);
        }

        // GET: api/Producttypes/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Producttype>> GetProducttype(int id)
        //{
        //    var producttype = await _context.Producttypes.FindAsync(id);

        //    if (producttype == null)
        //    {
        //        return NotFound();
        //    }

        //    return producttype;
        //}

        // PUT: api/Producttypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProducttype(int id, Producttype producttype)
        //{
        //    if (id != producttype.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(producttype).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProducttypeExists(id))
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

        // POST: api/Producttypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Producttype>> PostProducttype(Producttype producttype)
        {
            _context.Producttypes.Add(producttype);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducttype", new { id = producttype.Id }, producttype);
        }

        // DELETE: api/Producttypes/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducttype(int id)
        {
            var producttype = await _context.Producttypes.FindAsync(id);
            if (producttype == null)
            {
                return NotFound();
            }

            _context.Producttypes.Remove(producttype);
            await _context.SaveChangesAsync();

            return NoContent();
        }

       
    }
}
