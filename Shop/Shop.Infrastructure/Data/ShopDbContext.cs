using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure;

namespace Shop.Infrastructure.Data;

public partial class ShopDbContext : DbContext
{
    public ShopDbContext(DbContextOptions<ShopDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<DangKyTuVan> DangKyTuVans { get; set; }

    public virtual DbSet<DanhGium> DanhGia { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LoaiSua> LoaiSuas { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<SanPhamSua> SanPhamSuas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.MaCt).HasName("PK__ChiTietD__27258E74A135C457");

            entity.ToTable("ChiTietDonHang");

            entity.HasIndex(e => e.MaDh, "IX_ChiTietDonHang_MaDH");

            entity.HasIndex(e => e.MaSua, "IX_ChiTietDonHang_MaSua");

            entity.Property(e => e.MaCt).HasColumnName("MaCT");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaDh).HasColumnName("MaDH");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaDh)
                .HasConstraintName("FK__ChiTietDon__MaDH__46E78A0C");

            entity.HasOne(d => d.MaSuaNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaSua)
                .HasConstraintName("FK__ChiTietDo__MaSua__47DBAE45");
        });

        modelBuilder.Entity<DangKyTuVan>(entity =>
        {
            entity.HasKey(e => e.MaTuVan).HasName("PK__DangKyTu__AE2543CA7C7338C4");

            entity.ToTable("DangKyTuVan");

            entity.HasIndex(e => e.MaKh, "IX_DangKyTuVan_MaKh");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgayDangKy)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NoiDungTuVan).HasMaxLength(255);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DangKyTuVans).HasForeignKey(d => d.MaKh);
        });

        modelBuilder.Entity<DanhGium>(entity =>
        {
            entity.HasKey(e => e.MaDg).HasName("PK__DanhGia__27258660D39FE9C4");

            entity.HasIndex(e => e.MaNd, "IX_DanhGia_MaND");

            entity.HasIndex(e => e.MaSua, "IX_DanhGia_MaSua");

            entity.Property(e => e.MaDg).HasColumnName("MaDG");
            entity.Property(e => e.MaNd).HasColumnName("MaND");
            entity.Property(e => e.NgayDanhGia)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NoiDung).HasMaxLength(255);

            entity.HasOne(d => d.MaNdNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaNd)
                .HasConstraintName("FK__DanhGia__MaND__4CA06362");

            entity.HasOne(d => d.MaSuaNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaSua)
                .HasConstraintName("FK__DanhGia__MaSua__4D94879B");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDh).HasName("PK__DonHang__27258661DC37409C");

            entity.ToTable("DonHang");

            entity.HasIndex(e => e.MaKh, "IX_DonHang_MaKH");

            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__DonHang__MaKH__440B1D61");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGh).HasName("PK__GioHang__2725AE85E9FDE2C9");

            entity.ToTable("GioHang");

            entity.HasIndex(e => e.MaKh, "IX_GioHang_MaKH");

            entity.HasIndex(e => e.MaSua, "IX_GioHang_MaSua");

            entity.Property(e => e.MaGh).HasColumnName("MaGH");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__GioHang__MaKH__534D60F1");

            entity.HasOne(d => d.MaSuaNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaSua)
                .HasConstraintName("FK__GioHang__MaSua__5441852A");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KhachHan__2725CF1E8162DDA9");

            entity.ToTable("KhachHang");

            entity.HasIndex(e => e.MaNd, "IX_KhachHang_MaND");

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MaNd).HasColumnName("MaND");
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);

            entity.HasOne(d => d.MaNdNavigation).WithMany(p => p.KhachHangs)
                .HasForeignKey(d => d.MaNd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhachHang__MaND__3B75D760");
        });

        modelBuilder.Entity<LoaiSua>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__LoaiSua__730A5759DB06482B");

            entity.ToTable("LoaiSua");

            entity.Property(e => e.TenLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNd).HasName("PK__NguoiDun__2725D724E2A90582");

            entity.ToTable("NguoiDung");

            entity.Property(e => e.MaNd).HasColumnName("MaND");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.MatKhau).HasMaxLength(100);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.VaiTro).HasMaxLength(20);
        });

        modelBuilder.Entity<SanPhamSua>(entity =>
        {
            entity.HasKey(e => e.MaSua).HasName("PK__SanPhamS__318B29C4E3412C4F");

            entity.ToTable("SanPhamSua");

            entity.HasIndex(e => e.MaLoai, "IX_SanPhamSua_MaLoai");

            entity.Property(e => e.Gia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenSua).HasMaxLength(100);

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.SanPhamSuas)
                .HasForeignKey(d => d.MaLoai)
                .HasConstraintName("FK__SanPhamSu__MaLoa__403A8C7D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
