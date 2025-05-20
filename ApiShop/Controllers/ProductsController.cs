using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiShop.Model;
using ShopLib;
using Microsoft.AspNetCore.Authorization;

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

            ProductDTO result = new ProductDTO
            {
                Id=product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                TimeBought = product.TimeBought,
                TypeId = product.TypeId,
                BrandId = product.BrandId,

            };

            return Ok(result);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

            if (_context.Products.Contains(product)) return Ok(product.Id);
            else return BadRequest();
        }

        // DELETE: api/Products/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
