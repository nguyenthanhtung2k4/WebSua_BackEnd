using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs
{
    public class GetProductsAdminDTO
    {
        public int MaSua { get; set; }

        public string? TenSua { get; set; }

        public int? MaLoai { get; set; }

        public decimal? Gia { get; set; }

        public string? MoTa { get; set; }

        public int? SoLuong { get; set; }

        public string? HinhAnh { get; set; }
        public DateTime? NgayTao { get; set; }

    }
}
