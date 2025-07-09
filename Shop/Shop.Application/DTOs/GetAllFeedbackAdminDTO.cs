using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs
{
    public class GetAllFeedbackAdminDTO
    {
        public int MaDg { get; set; }
        public int? MaNd { get; set; }
        public int? MaSua { get; set; }

        public string? TenNguoiDung { get; set; }
        public string? HinhAnhNguoiDung { get; set; } // Renamed for clarity: user's avatar/image
        public string? TenSanPham { get; set; }
        public string? HinhAnhSanPham { get; set; } // Added to display product image if needed

        public string? NoiDung { get; set; }
        public int? Star { get; set; }
        public DateTime? NgayDanhGia { get; set; }
    }
}
