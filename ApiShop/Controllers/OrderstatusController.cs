using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiShop.Model;
using ShopLib;

namespace ApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderstatusController : ControllerBase
    {
        private readonly ShopdbContext _context;

        public OrderstatusController(ShopdbContext context)
        {
            _context = context;
        }

        // GET: api/Orderstatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orderstatus>>> GetOrderstatuses()
        {
            return await _context.Orderstatuses.ToListAsync();
        }

        // GET: api/Orderstatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orderstatus>> GetOrderstatus(int id)
        {
            var orderstatus = await _context.Orderstatuses.FindAsync(id);

            if (orderstatus == null)
            {
                return NotFound();
            }

            return orderstatus;
        }

        // PUT: api/Orderstatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderstatus(int id, Orderstatus orderstatus)
        {
            if (id != orderstatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderstatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderstatusExists(id))
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

        // POST: api/Orderstatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Orderstatus>> PostOrderstatus(Orderstatus orderstatus)
        {
            _context.Orderstatuses.Add(orderstatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderstatus", new { id = orderstatus.Id }, orderstatus);
        }

        // DELETE: api/Orderstatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderstatus(int id)
        {
            var orderstatus = await _context.Orderstatuses.FindAsync(id);
            if (orderstatus == null)
            {
                return NotFound();
            }

            _context.Orderstatuses.Remove(orderstatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderstatusExists(int id)
        {
            return _context.Orderstatuses.Any(e => e.Id == id);
        }
    }
}
