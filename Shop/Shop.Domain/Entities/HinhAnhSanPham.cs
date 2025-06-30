using System;

namespace Shop.Infrastructure
{
    public partial class HinhAnhSanPham
    {
        public int Id { get; set; }

        public int MaSanPham { get; set; } // Foreign key

        public string DuongDan { get; set; } = null!; // Đường dẫn ảnh

        // Navigation
        public virtual SanPhamSua? SanPham { get; set; }
    }
}
