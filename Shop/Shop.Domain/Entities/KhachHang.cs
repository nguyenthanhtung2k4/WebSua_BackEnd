using System;
using System.Collections.Generic;

namespace Shop.Infrastructure;

public partial class KhachHang
{
    public int MaKh { get; set; }

    public int MaNd { get; set; }

    public string? HoTen { get; set; }

    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }
    public string? GioiTinh { get; set; }
    public string? Image { get; set; }

    public virtual ICollection<DangKyTuVan> DangKyTuVans { get; set; } = new List<DangKyTuVan>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual NguoiDung MaNdNavigation { get; set; } = null!;
}
