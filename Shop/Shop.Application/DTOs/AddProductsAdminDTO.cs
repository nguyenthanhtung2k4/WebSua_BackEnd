using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shop.Application.DTOs
{
    public class AddProductsAdminDTO
    {
        public string? TenSua { get; set; }
        public int? MaLoai { get; set; }
        public decimal? Gia { get; set; }
        public string? MoTa { get; set; }
        public int? SoLuong { get; set; }

        public string? HinhAnh { get; set; } // Đường dẫn ảnh sẽ lưu trong DB
        public IFormFile? NewImageFile { get; set; } // File ảnh từ người dùng

        public string? Status { get; set; } = "active";
        public DateTime? NgayTao { get; set; } = DateTime.Now;
    }   
}
