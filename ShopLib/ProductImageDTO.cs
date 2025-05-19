﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLib
{
    public class ProductImageDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public byte[] Image { get; set; } = null!;
    }
}
