using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Infrastructure;

namespace Shop.Application.DTOs
{
    public class AddToCartDTO
    {
        public int MaSua { get; set; }
        public int SoLuong { get; set; }
    }
}
