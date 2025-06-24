using System;
using System.Collections.Generic;

namespace Shop.Infrastructure;

public partial class LoaiSua
{
    public int MaLoai { get; set; }

    public string? TenLoai { get; set; }

    public virtual ICollection<SanPhamSua> SanPhamSuas { get; set; } = new List<SanPhamSua>();
}
