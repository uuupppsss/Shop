using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLib
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int TypeId { get; set; }

        public int BrandId { get; set; }

        public int TimeBought { get; set; }

        public byte[]? HeaderImage { get; set; }

    }
}
