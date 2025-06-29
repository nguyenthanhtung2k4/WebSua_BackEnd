using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Infrastructure;

namespace Shop.Application.Services
{
    public class AdminProductsService  : IAdminProductsService
    {
        public readonly IRepository<SanPhamSua> _repository ;

        public AdminProductsService (IRepository<SanPhamSua> repository)
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
    }
}
