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
        public async Task<ActionResult<List<BasketItemDTO>>> GetUserBasketitems()
        {
            var us = HttpContext.User.Claims.First();
            int.TryParse(us.Value, out int user_id);
            //int user_id = 1;
            User user = await _context.Users.FirstOrDefaultAsync(u=>u.Id==user_id);
            if (user is null) return NotFound();

            List<Basketitem> basket = await _context.Basketitems.Include(i => i.Product)
                .Where(i => i.UserId == user_id).ToListAsync();

            List<BasketItemDTO> result = new();
            foreach(var item in basket)
            {
                int count = item.Count;
                bool able = false;
                Productsize productsize = await _context.Productsizes
                    .FirstOrDefaultAsync(i => i.ProductId == item.ProductId && i.Size == item.Size);
                if (item.Count > productsize.Count) count = productsize.Count;
                if (count > 0) able = true;
                result.Add(new BasketItemDTO
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Title,
                    Size = item.Size,
                    Count = count,
                    IfNotAbleToOrder = !able,
                    Cost=item.Product.Price*count,
                }); ;
            }
            return Ok(result);
            
        }

        [Authorize(Roles = "user")]
        [HttpGet("MaxCount/{id}")]
        public async Task<ActionResult<List<BasketItemDTO>>> GetMaxCount(int id)
        {
            var item = await _context.Basketitems.FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();
            var product = await _context.Products.Include(p => p.Productsizes)
                .FirstOrDefaultAsync(p=>p.Id==item.ProductId);
            return Ok(product.Productsizes.FirstOrDefault(s => s.Size == item.Size).Count);

        }
        // PUT: api/Basketitems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "user")]
        [HttpGet("Update/{id}/{count}")]
        public async Task<ActionResult> PutBasketitem(int id, int count)
        {
            Basketitem basketitem = await _context.Basketitems.FirstOrDefaultAsync(i => i.Id == id);
            if (basketitem is null) return NotFound();
            basketitem.Count = count;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
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

    }
}
