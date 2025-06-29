using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Infrastructure;
using Shop.Infrastructure.Data;


namespace Shop.Application.Services
{
    public class AdminUserService : IAdminUserService
    {
        public readonly IRepository<NguoiDung> _repository; 
        public AdminUserService(IRepository<NguoiDung> repository)
        {
            _repository = repository;
        }
        public readonly PasswordHasher<string> _hashPass = new();

        public async Task<IEnumerable<GetUserAdminDTO>> GetAllUsers()
        {
            var users = await _repository.GetAllAsync();
            var fileter = users
                .Where(x => x.status?.ToLower() != "deleted"); 




            return fileter.Select(x => new GetUserAdminDTO
            {
                MaNd = x.MaNd,
                TenDangNhap = x.TenDangNhap,
                Email = x.Email,
                VaiTro = x.VaiTro,
                NgayTao = x.NgayTao,
                status = x.status
            }).ToList();
        }

        public async Task<GetUserAdminDTO> GetUserById(int userId)
        {
            var user = await _repository.GetByIdAsync(userId); 
            if (user == null)
            {
                return null; 
            }

            return new GetUserAdminDTO
            {
                MaNd = user.MaNd,
                TenDangNhap = user.TenDangNhap,
                Email = user.Email,
                VaiTro = user.VaiTro,
                NgayTao = user.NgayTao,
             
                status = user.status
            };
        }

        public async Task<bool> UpdateUser(UpdateUserDTO updateDto)
        {
            var existingUser = await _repository.GetByIdAsync(updateDto.MaNd); // Đổi tên biến cho rõ ràng
            if (existingUser == null)
            {
                return false; // Người dùng không tồn tại
            }

            // Cập nhật các thuộc tính từ DTO sang entity
            existingUser.TenDangNhap = updateDto.TenDangNhap;
            existingUser.Email = updateDto.Email;
            existingUser.VaiTro = updateDto.VaiTro; // VaiTro từ DTO
            existingUser.status = updateDto.status; // status từ DTO

           
             //existingUser.MatKhau = updateDto.MatKhau; // Xóa hoặc comment dòng này

            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUser(AddUserAdminDTO addUserDTO)
        {
            var add = await _repository.FirstOrDefaultAsync(u => u.Email == addUserDTO.Email); 
            if (add != null) 
            {
                return false;
            }
            string hashedPassword = _hashPass.HashPassword(null, addUserDTO.MatKhau);
            var newUser = new NguoiDung
            {
                TenDangNhap = addUserDTO.TenDangNhap,
                // HASH MẬT KHẨU TRƯỚC KHI LƯU
                MatKhau = hashedPassword ,
                Email = addUserDTO.Email,
                VaiTro = addUserDTO.VaiTro ,
                status = addUserDTO.status ,
                //avata = addUserDTO.avata ,
            }; 
            await _repository.AddAsync(newUser);
            await _repository.SaveChangesAsync();
            return true;
        }

         public async Task<bool> DeleteUser(int id)
        {
            var existingUser = await _repository.GetByIdAsync(id); 
            if (existingUser == null)
            {
                return false; 
            }

          
            existingUser.status = "deleted"; // status từ DTO


            //existingUser.MatKhau = updateDto.MatKhau; // Xóa hoặc comment dòng này

            await _repository.SaveChangesAsync();
            return true;
        }

    }
}