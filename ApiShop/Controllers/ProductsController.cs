﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiShop.Model;
using ShopLib;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SignalR.Client;

namespace ApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopdbContext _context;

        public ProductsController(ShopdbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet("{filterword}/{type_id}/{brand_id}")]
        public async Task<ActionResult<List<Product>>> GetProducts(string filterword,int type_id,int brand_id)
        {
            List<ProductDTO> result = new();
            var full_products_list = await _context.Products.Include(p => p.Productimages).ToListAsync();
            if (filterword != "-")
            {
                full_products_list = full_products_list.
                   Where(p => p.Title.ToLower().Contains(filterword) ||
               (p.Description != null && p.Description.ToLower().Contains(filterword))).ToList();
            }
            if (type_id > 0)
            {
                full_products_list = full_products_list.
                    Where(p=>p.TypeId==type_id).ToList();
            }
            if (brand_id > 0)
            {
                full_products_list = full_products_list.
                    Where(p => p.BrandId == brand_id).ToList();
            }

            foreach (var item in full_products_list)
            {
                result.Add(new ProductDTO
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Price = item.Price,
                    TimeBought = item.TimeBought,
                    TypeId = item.TypeId,
                    BrandId = item.BrandId,
                    HeaderImage = item.Productimages.Count==0? null: item.Productimages.First().Image,
                });
            }
            return Ok(result);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            var brand=await _context.Brands.FindAsync(product.BrandId);
            if (brand == null) return NotFound();

            var type=await _context.Producttypes.FindAsync(product.TypeId);
            if (type == null) return NotFound();

            ProductDTO result = new ProductDTO
            {
                Id=product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                TimeBought = product.TimeBought,
                TypeId = product.TypeId,
                BrandId = product.BrandId,
                BrandTitle=brand.Title,
                TypeTitle=type.Title,

            };

            return Ok(result);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<ActionResult> PutProduct(ProductDTO product)
        {
            var found_product=await _context.Products.FirstOrDefaultAsync(p=>p.Id==product.Id);
            if(found_product == null) return NotFound();

            found_product.Price = product.Price;
            found_product.Description = product.Description;

            try
            {
                await _context.SaveChangesAsync();
                HubConnection connection = new HubConnectionBuilder()
                       .WithUrl("http://localhost:5226/clientshub").Build();
                await connection.StartAsync();
                await connection.SendAsync("ProductUpdated", product.Id);
                await connection.StopAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<int>> PostProduct(ProductDTO sent_product)
        {
            Product product = new Product()
            {
                Title = sent_product.Title,
                Description = sent_product.Description,
                Price = sent_product.Price,
                BrandId = sent_product.BrandId,
                TypeId = sent_product.TypeId,
                TimeBought = 0,
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
            

            if (_context.Products.Contains(product)) return Ok(product.Id);
            else return BadRequest();
        }

        // DELETE: api/Products/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p=>p.Id==id);
            if (product == null) return NotFound();

            var orderitems = await _context.Orderitems.Include(i => i.Order)
                .Where(i => i.ProductId == id).ToListAsync();

            foreach (var i in orderitems)
            {
                if(i.Order.StatusId<6)
                {
                    return BadRequest("Вы не можете удалить продукт у когорого есть активные заказы");
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            HubConnection connection = new HubConnectionBuilder()
                          .WithUrl("http://localhost:5226/clientshub").Build();
            await connection.StartAsync();
            await connection.SendAsync("ProductDeleted",id);
            await connection.StopAsync();

            return Ok();
        }

       
    }
}
