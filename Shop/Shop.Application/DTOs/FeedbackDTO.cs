using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs
{
    public class FeedbackDTO
    {
        public int MaSua { get; set; }
        public string TenNguoiDung { get; set; } = string.Empty;
        public string NoiDung { get; set; } = string.Empty;
        public int Star { get; set; }
    }
}
