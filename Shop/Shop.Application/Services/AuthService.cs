using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Infrastructure;
using Shop.Infrastructure.Data;

namespace Shop.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ShopDbContext _context;
        public readonly PasswordHasher<string> _hashPass = new();

        public AuthService(ShopDbContext context)
        {
            _context = context;
        }

        // Đăng ký tài khoản mới
        public async Task<bool> Register(RegisterDTOs dtos)
        {
            // Kiểm tra trùng tên đăng nhập
            var existingUser = await _context.NguoiDungs
                .FirstOrDefaultAsync(x => x.Email == dtos.Email);

            if (existingUser != null)
                return false; // Đã tồn tại

            // Ma Hoa pass 
            string hashedPassword = _hashPass.HashPassword(null, dtos.MatKhau);

            var nguoiDung = new NguoiDung
            {
                TenDangNhap = dtos.TenDangNhap,
                MatKhau = hashedPassword, // ma hoa  passs
                Email = dtos.Email,
                VaiTro = dtos.VaiTro ?? "KhachHang",
                NgayTao = DateTime.Now
            };

            _context.NguoiDungs.Add(nguoiDung);
            await _context.SaveChangesAsync();
            return true;
        }

        // Đăng nhập
        public async Task<bool> Login(string email, string password)
        {
            var user = await _context.NguoiDungs
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null) return false;

            // Kiểm tra mật khẩu
            var result = _hashPass.VerifyHashedPassword(null, user.MatKhau, password);

            return result == PasswordVerificationResult.Success;
        }

    }
}
