using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs
{
    public class GetImageProductsAdminDTO
    {
        public int Id { get; set; }

        public int MaSanPham { get; set; } // Foreign key

        public string DuongDan { get; set; } = null!; // Đường dẫn ảnh

        // Navigation
        public virtual SanPhamSua? SanPham { get; set; }
    }
}
