using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLib
{
    public class ProductSizeDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Size { get; set; } = null!;

        public int Count { get; set; } 
    }
}
