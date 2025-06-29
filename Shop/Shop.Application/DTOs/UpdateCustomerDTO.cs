using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace Shop.Application.DTOs;
public class UpdateCustomerDTO
{
    public int MaKh { get; set; }

    public string? HoTen { get; set; }
    public string? SoDienThoai { get; set; }
    public string? DiaChi { get; set; }
    public string? GioiTinh { get; set; }

    public string? CurrentImagePath { get; set; } // ảnh hiện tại từ DB
    public IFormFile? NewImageFile { get; set; }   // ảnh mới được upload
}
