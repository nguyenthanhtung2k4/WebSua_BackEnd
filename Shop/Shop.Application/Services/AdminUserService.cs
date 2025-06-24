using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Infrastructure.Data;

namespace Shop.Application.Services
{
    public class AdminUserService : IAdminUserService
    {
        public readonly ShopDbContext _context;
        public AdminUserService (ShopDbContext  context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GetUserAdminDTO>> GetAllUsers()
        {
            return await _context.NguoiDungs
                .Select(u => new GetUserAdminDTO
            {
                    MaNd = u.MaNd,
                    TenDangNhap =u.TenDangNhap,
                    Email = u.Email,
                    VaiTro= u.VaiTro,
                    NgayTao = u.NgayTao,
                    avata = u.status,
            }).ToListAsync();
        }
        //public async Task<GetUserAdminDTO> GetUserById(int userId);
        //public async Task<bool> UpdateUser(UpdateUserDTO updateDto); // Update user details/roles
        //public async Task<bool> DeleteUser(int userId);
        //public async Task<bool> AddUser(AddUserAdminDTO addUserDTO);
    }
}
