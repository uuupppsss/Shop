﻿using System;
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
using System.Drawing.Drawing2D;

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
                    Id = item.Id,
                    Title = item.Title,
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
        [HttpGet("{title}")]
        public async Task<ActionResult> PostProducttype(string title)
        {
            _context.Producttypes.Add(new Producttype
            {
                Title = title
            });
            try
            {
                await _context.SaveChangesAsync();
                HubConnection connection = new HubConnectionBuilder()
                           .WithUrl("http://localhost:5226/clientshub").Build();
                await connection.StartAsync();
                await connection.SendAsync("TypesUpdated");
                await connection.StopAsync();
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        // DELETE: api/Producttypes/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProducttype(int id)
        {
            var producttype = await _context.Producttypes.Include(t=>t.Products)
                .FirstOrDefaultAsync(y=>y.Id==id);
            if (producttype == null)
            {
                return NotFound();
            }
            if (producttype.Products.Count > 0)
                return BadRequest("Вы не можете удалить категорию у которой есть подукты");

            _context.Producttypes.Remove(producttype);
            await _context.SaveChangesAsync();

            HubConnection connection = new HubConnectionBuilder()
                           .WithUrl("http://localhost:5226/clientshub").Build();
            await connection.StartAsync();
            await connection.SendAsync("TypesUpdated");
            foreach (var p in producttype.Products)
            {
                await connection.SendAsync("ProductDeleted", p.Id);
            }
            await connection.StopAsync();

            return Ok();
        }

       
    }
}
