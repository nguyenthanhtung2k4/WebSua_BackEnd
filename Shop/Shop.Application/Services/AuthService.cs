using System; 
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Infrastructure; 

namespace Shop.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<NguoiDung> _repository; 
        public readonly PasswordHasher<string> _hashPass = new();

        public AuthService(IRepository<NguoiDung> repository)
        {
            _repository = repository;
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
                // HASH MẬT KHẨU TRƯỚC KHI LƯU
                MatKhau = hashedPassword,
                Email = dtos.Email,
                VaiTro = dtos.VaiTro,
                status = dtos.status,
                //avata = dtos.avata,
            };
            await _repository.AddAsync(newUser);
            int savedChanges  = await _repository.SaveChangesAsync();
            return savedChanges > 0; 
        }

        
        public async Task<bool> Login(string email, string password)
        {
            
            var user = await _repository.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return false; 
            }

            
            var result = _hashPass.VerifyHashedPassword(null, user.MatKhau, password);

            
            return result == PasswordVerificationResult.Success;
        }
    }
}