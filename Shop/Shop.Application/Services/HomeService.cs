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
    public class HomeService : IHomeService
    {
        private readonly IRepository<SanPhamSua> _repository;
        private readonly IRepository<HinhAnhSanPham> _imageRepository;
        private readonly IRepository<LoaiSua> _loaiRepository;

        public HomeService(
            IRepository<SanPhamSua> repository,
            IRepository<HinhAnhSanPham> imageRepository,
            IRepository<LoaiSua> loaiRepository) 
        {
            _repository = repository;
            _imageRepository = imageRepository;
            _loaiRepository = loaiRepository;
        }

        public async Task<IEnumerable<GetProductsAdminDTO>> GetAllProducts()
        {
            var products = await _repository.GetAllAsync();
            var filtered = products.Where(x => x.Status?.ToLower() != "deleted");

            var loai = await _loaiRepository.GetAllAsync();
            var allImages = await _imageRepository.GetAllAsync();

            var result = filtered.Select(x =>
            {
                var firstImage = allImages
                    .Where(img => img.MaSanPham == x.MaSua)
                    .Select(img => img.DuongDan)
                    .FirstOrDefault();
                var idLoai = loai
                    .Where(l => l.MaLoai == x.MaLoai)
                    .Select(l => l.TenLoai)
                    .FirstOrDefault() ?? "other";


                return new GetProductsAdminDTO
                {
                    MaSua = x.MaSua,
                    TenSua = x.TenSua,
                    TenLoai=idLoai ,
                    Gia = x.Gia,
                    SoLuong = x.SoLuong,
                    NgayTao = x.NgayTao,
                    MoTa = x.MoTa,
                    Status = x.Status,
                    HinhAnhs = firstImage != null ? new List<string> { firstImage } : new List<string>() // chỉ 1 ảnh
                };
            });

            return result.ToList();
        }

        public async Task<GetProductsAdminDTO?> GetIdProduct(int productId)
        {
            var product = await _repository.FirstOrDefaultAsync(x => x.MaSua == productId);
            if (product == null || product.Status?.ToLower() == "deleted")
                return null;

            var images = (await _imageRepository.FindAsync(x => x.MaSanPham == productId)).ToList(); 
            var loai = await _loaiRepository.FirstOrDefaultAsync(x => x.MaLoai == product.MaLoai);

            return new GetProductsAdminDTO
            {
                MaSua = product.MaSua,
                TenSua = product.TenSua,
                TenLoai = loai?.TenLoai ?? "Khong do",
                Gia = product.Gia,
                MoTa = product.MoTa,
                HinhAnhs = images.Select(img => img.DuongDan).ToList(), // ✅ an toàn
                NgayTao = product.NgayTao,
                Status = product.Status,
                SoLuong = product.SoLuong
            };
        }



    }
}
