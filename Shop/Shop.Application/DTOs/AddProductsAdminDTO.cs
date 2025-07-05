using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shop.Application.DTOs
{
    public class AddProductsAdminDTO
    {
        [Required]
        public string? TenSua { get; set; }

        public int? MaLoai { get; set; }

        public decimal? Gia { get; set; }

        public string? MoTa { get; set; }

        public int? SoLuong { get; set; }

        public string? Status { get; set; } = "active";

        public DateTime? NgayTao { get; set; } = DateTime.Now;

        public List<IFormFile>? NewImageFiles { get; set; }

        public List<string>? HinhAnhs { get; set; } // Lưu đường dẫn ảnh sau khi upload
    }
}
