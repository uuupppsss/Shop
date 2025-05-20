using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLib
{
    public class BasketItemDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        public int Count { get; set; }

        public string Size { get; set; } = null!;

        public string ProductName { get; set; }=string.Empty;
    }
}
