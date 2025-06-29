using System;
using System.Collections.Generic;

namespace Shop.Infrastructure;

public partial class SanPhamSua
{
    public int MaSua { get; set; }

    public string? TenSua { get; set; }

    public int? MaLoai { get; set; }

    public decimal? Gia { get; set; }

    public string? MoTa { get; set; }

    public int? SoLuong { get; set; }

    public string? HinhAnh { get; set; }

    public string? Status { get; set; }
    public DateTime? NgayTao { get; set; }
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual LoaiSua? MaLoaiNavigation { get; set; }
}
