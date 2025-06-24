using System;
using System.Collections.Generic;

namespace Shop.Infrastructure;

public partial class DanhGium
{
    public int MaDg { get; set; }

    public int? MaNd { get; set; }

    public int? MaSua { get; set; }

    public string? NoiDung { get; set; }

    public int? Star { get; set; }

    public DateTime? NgayDanhGia { get; set; }

    public virtual NguoiDung? MaNdNavigation { get; set; }

    public virtual SanPhamSua? MaSuaNavigation { get; set; }
}
