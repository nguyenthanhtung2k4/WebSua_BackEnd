using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Infrastructure;
using Shop.Infrastructure.Data; // Thêm namespace này nếu NguoiDung nằm trong đây

namespace Shop.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<NguoiDung> _repository;
        private readonly IRepository<KhachHang> _CustomerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly PasswordHasher<string> _hashPass = new();

        public AuthService(IRepository<NguoiDung> repository, IRepository<KhachHang> customerRepository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _CustomerRepository = customerRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Register(RegisterDTOs dtos)
        {
            var existingUser = await _repository.FirstOrDefaultAsync(x => x.Email == dtos.Email);

            if (existingUser != null)
            {
            
                return false;
            }

            string hashedPassword = _hashPass.HashPassword(null, dtos.MatKhau);

            var newUser = new NguoiDung
            {
                TenDangNhap = dtos.TenDangNhap,
                MatKhau = hashedPassword,
                Email = dtos.Email,
                VaiTro = dtos.VaiTro, 
                status = dtos.status,
               
            };

            await _repository.AddAsync(newUser);
            int savedChanges = await _repository.SaveChangesAsync();

            
            if (newUser.VaiTro == "KhachHang")
            { 
                var customer = await _repository.FirstOrDefaultAsync(x => x.Email == dtos.Email);
                if (customer != null)
                {
                    var newCustomer = new KhachHang
                    {
                        MaNd = customer.MaNd,
                        HoTen = customer.TenDangNhap
                    };
                    await _CustomerRepository.AddAsync(newCustomer);
                    await _CustomerRepository.SaveChangesAsync();
                }
            }

            return savedChanges > 0;
        }

        // Thay đổi kiểu trả về để bao gồm VaiTro
        public async Task<LoginResultDTO> Login(string email, string password)
        {
            var user = await _repository.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return new LoginResultDTO { Success = false, Role = null }; // Không tìm thấy người dùng
            }

            var result = _hashPass.VerifyHashedPassword(null, user.MatKhau, password);
            if (result == PasswordVerificationResult.Success)
            {
                var session = _httpContextAccessor.HttpContext?.Session;
                if (session != null)
                {
                    session.SetString("Email", user.Email!);
                    session.SetInt32("MaND", user.MaNd); // Dùng khi cần ID người dùng
                    session.SetString("VaiTro", user.VaiTro!); // Lưu VaiTro vào session
                }
                return new LoginResultDTO { Success = true, Role = user.VaiTro }; // Đăng nhập thành công, trả về vai trò
            }

            return new LoginResultDTO { Success = false, Role = null }; // Mật khẩu không đúng
        }
    }
}