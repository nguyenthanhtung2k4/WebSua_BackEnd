using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Infrastructure;

namespace Shop.Application.Services
{
    public class AdminCustomerService : IAdminCustomerService
    {
        public readonly IRepository<KhachHang> _repository;
        public AdminCustomerService(IRepository<KhachHang> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GetCustomerAdminDTO>> GetAllCustomer()
        {
            var customer = await _repository.GetAllIncludingAsync(x=> x.MaNdNavigation);
            var fileter = customer
                .Where(x => x.MaNdNavigation.status?.ToLower() != "deleted");
            return fileter.Select(x => new GetCustomerAdminDTO
            {
                MaKh=x.MaKh,
                HoTen=x.HoTen,
                GioiTinh=x.GioiTinh,
                NguoiDungs= new GetUserAdminDTO
                {
                    Email=x.MaNdNavigation.Email,
                    NgayTao=x.MaNdNavigation.NgayTao
                }
            }).ToList();
        }


        public async Task<GetCustomerAdminDTO?> GetCustomerById(int userId)
        {
            var customer = await _repository.GetAllIncludingAsync(x => x.MaNdNavigation);
            var result = customer.FirstOrDefault(x => x.MaKh == userId);

            if (result == null) return null;

            return new GetCustomerAdminDTO
            {
                MaKh = result.MaKh,
                //MaNd = result.MaNd,
                HoTen = result.HoTen,
                GioiTinh = result.GioiTinh,
                DiaChi = result.DiaChi,
                SoDienThoai = result.SoDienThoai,
                Image = result.Image,

                NguoiDungs = result.MaNdNavigation == null ? null : new GetUserAdminDTO
                {
                    //MaNd = result.MaNdNavigation.MaNd,
                    TenDangNhap = result.MaNdNavigation.TenDangNhap,
                    Email = result.MaNdNavigation.Email,
                    VaiTro = result.MaNdNavigation.VaiTro,
                    NgayTao = result.MaNdNavigation.NgayTao,
                    status = result.MaNdNavigation.status
                }
            };
        }

        public async Task<bool> UpdateCustomer(UpdateCustomerDTO updateDto)
        {
            var existingCustomer = await _repository.GetByIdAsync(updateDto.MaKh);
            if (existingCustomer == null) return false;

            existingCustomer.HoTen = updateDto.HoTen;
            existingCustomer.SoDienThoai = updateDto.SoDienThoai;
            existingCustomer.DiaChi = updateDto.DiaChi;
            existingCustomer.GioiTinh = updateDto.GioiTinh;
            existingCustomer.Image = updateDto.CurrentImagePath;

            await _repository.SaveChangesAsync();
            return true;
        }

    }

}

