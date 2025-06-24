using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs
{
    public class RegisterDTOs
    {
        //public int MaNd { get; set; }

        public string TenDangNhap { get; set; } = null!;

        public string MatKhau { get; set; } = null!;

        public string? Email { get; set; }

        public string? VaiTro { get; set; } = "KhachHang";

        //public DateTime? NgayTao { get; set; }

        public string? Face { get; set; } = string.Empty;
    }
}
