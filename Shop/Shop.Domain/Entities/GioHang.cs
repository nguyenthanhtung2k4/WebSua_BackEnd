using System;
using System.Collections.Generic;

namespace Shop.Infrastructure;

public partial class GioHang
{
    public int MaGh { get; set; }

    public int? MaKh { get; set; }

    public int? MaSua { get; set; }

    public int? SoLuong { get; set; }

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual SanPhamSua? MaSuaNavigation { get; set; }
}
