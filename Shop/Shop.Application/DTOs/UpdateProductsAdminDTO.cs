using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shop.Application.DTOs
{
    public class UpdateProductsAdminDTO
    {
        [Required]
        public int?  MaSua { get; set; }  
        public string? TenSua { get; set; }

        public int? MaLoai { get; set; }

        public decimal? Gia { get; set; }

        public string? MoTa { get; set; }

        public int? SoLuong { get; set; }

        public string? Status { get; set; }

        public DateTime? NgayTao { get; set; } = DateTime.Now;

        public List<IFormFile>? NewImageFiles { get; set; }

        public List<string>? HinhAnhs { get; set; } // Lưu đường dẫn ảnh sau khi upload
    }
}
