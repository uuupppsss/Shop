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
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

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

        [Authorize(Roles = "user")]
        [HttpGet("User")]
        public async Task<ActionResult<List<OrderDTO>>> GetUserOrders()
        {
            var us = HttpContext.User.Claims.First();
            int.TryParse(us.Value, out int user_id);
            if (user_id==0) return NotFound();
            List<OrderDTO> result = new();
            var orders=await _context.Orders.Include(o => o.Status).Where(o => o.UserId == user_id).OrderByDescending(o=>o.CreateDate).ToListAsync();
            foreach (var item in orders) 
            {
                result.Add(new OrderDTO
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    StatusId = item.StatusId,
                    CreateDate = item.CreateDate,
                    Adress = item.Adress,
                    Index = item.Index,
                    ContactPhone = item.ContactPhone,
                    FullName = item.FullName,
                    Trak = item.Trak,
                    Cost = item.Cost,
                    Status = item.Status.Title,
                });
                
            }
            return Ok(result);
        }


        // GET: api/Orders/5
        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        [HttpGet("Items/{id}")]
        public async Task<ActionResult<List<OrderItemDTO>>> GetOrderItems(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            var orderitems=await _context.Orderitems.Where(i=>i.OrderId==id).Include(i=>i.Product).ToListAsync();
            List<OrderItemDTO> result = new();
            foreach (var item in orderitems)
            {
                result.Add(new OrderItemDTO
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Count = item.Count,
                    OrderId = item.Id,
                    Size = item.Size,
                    ProductName=item.Product.Title
                });
            }


            return Ok(result);

        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpGet("Update/{order_id}/{status_id}/{trak}")]
        public async Task<ActionResult> PutOrder(int order_id,int status_id,string trak)
        {
            var order=await _context.Orders.FindAsync(order_id);
            if (order == null) return NotFound();
            order.StatusId = status_id;
            if(trak !="-") order.Trak = trak;

            try
            {
                await _context.SaveChangesAsync();
                HubConnection connection = new HubConnectionBuilder()
                           .WithUrl("http://localhost:5226/clientshub").Build();
                await connection.StartAsync();
                await connection.SendAsync("OrderUpdated", order_id);
                await connection.StopAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "user")]
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderDTO sent_order)
        {
            var us = HttpContext.User.Claims.First();
            int.TryParse(us.Value, out int user_id);
            if (user_id == 0) return NotFound();
            var order = new Order
            {
                UserId= user_id,
                CreateDate = sent_order.CreateDate,
                Cost= sent_order.Cost,
                StatusId=1,
                Adress=sent_order.Adress,
                Index= sent_order.Index,
                ContactPhone=sent_order.ContactPhone,
                Trak=sent_order.Trak,
                FullName=sent_order.FullName,
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var basket = await _context.Basketitems.Where(i => i.UserId == user_id).ToListAsync();
            foreach(var i in basket)
            {
                _context.Orderitems.Add(new Orderitem
                {
                    ProductId=i.ProductId,
                    Size = i.Size,
                    Count= i.Count,
                    OrderId=order.Id,
                });
                _context.Basketitems.Remove(i);
            }

            try
            {
                await _context.SaveChangesAsync();
                HubConnection connection = new HubConnectionBuilder()
                           .WithUrl("http://localhost:5226/adminshub").Build();
                await connection.StartAsync();
                await connection.SendAsync("OrderCreated");
                await connection.StopAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

       
    }
}
