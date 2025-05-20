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
    public class OrdersController : ControllerBase
    {
        private readonly ShopdbContext _context;

        public OrdersController(ShopdbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [Authorize (Roles ="admin")]
        [HttpGet("List/{status_id}")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders(int status_id)
        {
            List<OrderDTO> result = new();
            foreach (var item in _context.Orders.Include(o=>o.Status))
            {
                if(status_id == 0||item.StatusId==status_id)
                {
                    result.Add(new OrderDTO
                    {
                        Id= item.Id,
                        UserId=item.UserId,
                        StatusId=item.StatusId,
                        CreateDate= item.CreateDate,
                        Adress= item.Adress,
                        Index=item.Index,
                        ContactPhone=item.ContactPhone,
                        FullName=item.FullName,
                        Trak=item.Trak,
                        Cost= item.Cost,
                        Status=item.Status.Title,
                    });
                }
            }
            return Ok(result);
        }

        // GET: api/Orders/5
        [Authorize(Roles = "user,admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _context.Orders.Include(o=>o.Status).FirstOrDefaultAsync(o=>o.Id==id);
            if (order == null) return NotFound();

            OrderDTO result = new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                StatusId = order.StatusId,
                CreateDate = order.CreateDate,
                Adress = order.Adress,
                Index = order.Index,
                ContactPhone = order.ContactPhone,
                FullName = order.FullName,
                Trak = order.Trak,
                Cost = order.Cost,
                Status = order.Status.Title,
            };

            return Ok(result);
            
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "user")]
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
