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
    public class BasketitemsController : ControllerBase
    {
        private readonly ShopdbContext _context;

        public BasketitemsController(ShopdbContext context)
        {
            _context = context;
        }

        // GET: api/Basketitems
        [Authorize (Roles ="user")]
        [HttpGet]
        public async Task<ActionResult<List<Basketitem>>> GetBasketitems()
        {
            return await _context.Basketitems.ToListAsync();
        }

        // GET: api/Basketitems/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Basketitem>> GetBasketitem(int id)
        //{
        //    var basketitem = await _context.Basketitems.FindAsync(id);

        //    if (basketitem == null)
        //    {
        //        return NotFound();
        //    }

        //    return basketitem;
        //}

        // PUT: api/Basketitems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "user")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasketitem(int id, Basketitem basketitem)
        {
            if (id != basketitem.Id)
            {
                return BadRequest();
            }

            _context.Entry(basketitem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BasketitemExists(id))
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

        // POST: api/Basketitems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "user")]
        [HttpGet("Create/{product_id}/{size}")]
        public async Task<ActionResult<Basketitem>> CreateBasketitem(int product_id,string size)
        {
            var us = HttpContext.User.Claims.First();
            int.TryParse(us.Value, out int user_id);
            if(user_id==0) return Unauthorized();
            Basketitem basketitem = new Basketitem
            {
                ProductId=product_id,
                Size=size,
                UserId=user_id,
                Count=1
            };
            _context.Basketitems.Add(basketitem);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Basketitems/5
        [Authorize(Roles = "user")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketitem(int id)
        {
            var basketitem = await _context.Basketitems.FindAsync(id);
            if (basketitem == null)
            {
                return NotFound();
            }

            _context.Basketitems.Remove(basketitem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BasketitemExists(int id)
        {
            return _context.Basketitems.Any(e => e.Id == id);
        }
    }
}
