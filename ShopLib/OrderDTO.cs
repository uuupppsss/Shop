using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLib
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime CreateDate { get; set; }

        public decimal Cost { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; } = string.Empty;

        public string Adress { get; set; } = null!;

        public string Index { get; set; } = null!;

        public string ContactPhone { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Trak { get; set; } 
    }
}
