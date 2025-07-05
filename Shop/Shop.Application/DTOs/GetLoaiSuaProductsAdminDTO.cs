using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs
{
    public class GetLoaiSuaProductsAdminDTO
    {
        public int MaLoai { get; set; }

        public string? TenLoai { get; set; }

        //public virtual ICollection<SanPhamSua> SanPhamSuas { get; set; } = new List<SanPhamSua>();
    }
}
