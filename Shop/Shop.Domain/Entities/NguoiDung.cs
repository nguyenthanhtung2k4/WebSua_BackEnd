using System;
using System.Collections.Generic;

namespace Shop.Infrastructure;

public partial class NguoiDung
{
    public int MaNd { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string? Email { get; set; }

    public string? VaiTro { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? avata { get; set; }
    public string? status  { get; set; }
    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();
}
