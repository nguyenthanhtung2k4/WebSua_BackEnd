using System;
using System.Collections.Generic;

namespace Shop.Infrastructure;

public partial class DangKyTuVan
{
    public int MaTuVan { get; set; }

    public string? HoTen { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }
    public string? sex { get; set; }

    public string? NoiDungTuVan { get; set; }

    public DateTime? NgayDangKy { get; set; }

    public string? TrangThai { get; set; }

    public int? MaKh { get; set; }

    public virtual KhachHang? MaKhNavigation { get; set; }
}
