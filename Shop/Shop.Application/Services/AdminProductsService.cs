using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Infrastructure;

namespace Shop.Application.Services
{
    public class AdminProductsService  : IAdminProductsService
    {
        public readonly IRepository<SanPhamSua> _repository ;
        

        public AdminProductsService (IRepository<SanPhamSua> repository )
        {
            _repository = repository;
        }


        public async Task<IEnumerable<GetProductsAdminDTO>> GetAllProducts()
        {
            var customer = await _repository.GetAllAsync();
            var fileter = customer
                .Where(x => x.Status?.ToLower() != "deleted");
            return fileter.Select(x => new GetProductsAdminDTO
            {
              MaSua=x.MaSua,
              TenSua=x.TenSua,
              Gia=x.Gia,
              SoLuong=x.SoLuong,

            }).ToList();
        }

        public async Task<bool> AddProduct(AddProductsAdminDTO addUserDTO)
        {
            // Kiểm tra sản phẩm đã tồn tại chưa
            var existingProduct = await _repository.FirstOrDefaultAsync(x => x.TenSua == addUserDTO.TenSua);
            if (existingProduct != null)
            {
                return false; 
            }

            
            var product = new SanPhamSua
            {
                TenSua = addUserDTO.TenSua,
                MaLoai = addUserDTO.MaLoai,
                Gia = addUserDTO.Gia,
                MoTa = addUserDTO.MoTa,
                SoLuong = addUserDTO.SoLuong,
                HinhAnh = addUserDTO.HinhAnh,
                Status = addUserDTO.Status,
                NgayTao = addUserDTO.NgayTao ?? DateTime.Now
            };

            await _repository.AddAsync(product);          
            await _repository.SaveChangesAsync();   

            return true;
        }



    }
}
