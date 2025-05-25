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
    public class BrandsController : ControllerBase
    {
        private readonly ShopdbContext _context;

        public BrandsController(ShopdbContext context)
        {
            _context = context;
        }

        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<List<BrandDTO>>> GetBrands()
        {
            List<BrandDTO> result = new();
            foreach(var item in _context.Brands)
            {
                result.Add(new BrandDTO
                {
                    Id = item.Id,
                    Title = item.Title,
                });
            }
            return Ok(result);
        }

        //// GET: api/Brands/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Brand>> GetBrand(int id)
        //{
        //    var brand = await _context.Brands.FindAsync(id);

        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }

        //    return brand;
        //}

        //// PUT: api/Brands/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBrand(int id, Brand brand)
        //{
        //    if (id != brand.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(brand).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BrandExists(id))
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

        // POST: api/Brands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [Authorize(Roles = "admin")]
        [HttpGet("{title}")]
        public async Task<ActionResult> PostBrand(string title)
        {
            _context.Brands.Add(new Brand
            {
                Title= title
            });
            try
            {
                await _context.SaveChangesAsync();
                HubConnection connection = new HubConnectionBuilder()
                           .WithUrl("http://localhost:5226/clientshub").Build();
                await connection.StartAsync();
                await connection.SendAsync("BrandsUpdated");
                await connection.StopAsync();
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var brand = await _context.Brands.Include(b=>b.Products)
                .FirstOrDefaultAsync(b=>b.Id==id);
            if (brand == null)
            {
                return NotFound();
            }
            if (brand.Products.Count > 0)
                return BadRequest("Вы не можете удалить бренд у которого есть подукты");

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            HubConnection connection = new HubConnectionBuilder()
                           .WithUrl("http://localhost:5226/clientshub").Build();
            await connection.StartAsync();
            await connection.SendAsync("BrandsUpdated");
            foreach (var p in brand.Products)
            {
                await connection.SendAsync("ProductDeleted", p.Id);
            }
            await connection.StopAsync();

            return Ok();
        }
    }
}
