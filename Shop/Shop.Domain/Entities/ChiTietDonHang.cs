using System;
using System.Collections.Generic;

namespace Shop.Infrastructure;

public partial class ChiTietDonHang
{
    public int MaCt { get; set; }

    public int? MaDh { get; set; }

    public int? MaSua { get; set; }

    public int? SoLuong { get; set; }

    public decimal? DonGia { get; set; }

    public virtual DonHang? MaDhNavigation { get; set; }

    public virtual SanPhamSua? MaSuaNavigation { get; set; }
}
