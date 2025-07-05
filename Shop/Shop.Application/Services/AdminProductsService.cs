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
using static System.Net.Mime.MediaTypeNames;


namespace Shop.Application.Services
{
    public class AdminProductsService : IAdminProductsService
    {
        private readonly IRepository<SanPhamSua> _repository;
        private readonly IRepository<HinhAnhSanPham> _imageRepository;
        private readonly IRepository<LoaiSua> _loaiRepository; 

        public AdminProductsService(
            IRepository<SanPhamSua> repository,
            IRepository<HinhAnhSanPham> imageRepository,
            IRepository<LoaiSua> loaiRepository) // ✅ FIX: Nhận thêm từ DI
        {
            _repository = repository;
            _imageRepository = imageRepository;
            _loaiRepository = loaiRepository;
        }

        public async Task<IEnumerable<GetProductsAdminDTO>> GetAllProducts()
        {
            var products = await _repository.GetAllAsync();
            var filtered = products.Where(x => x.Status?.ToLower() != "deleted");

            return filtered.Select(x => new GetProductsAdminDTO
            {
                MaSua = x.MaSua,
                TenSua = x.TenSua,
                Gia = x.Gia,
                SoLuong = x.SoLuong,
                NgayTao = x.NgayTao
            }).ToList();
        }

        public async Task<bool> AddProduct(AddProductsAdminDTO dto)
        {
            var exists = await _repository.FirstOrDefaultAsync(x => x.TenSua == dto.TenSua);
            if (exists != null) return false;

            var product = new SanPhamSua
            {
                
                TenSua = dto.TenSua,
                MaLoai = dto.MaLoai,
                Gia = dto.Gia,
                MoTa = dto.MoTa,
                SoLuong = dto.SoLuong,
                Status = dto.Status ?? "active",
                NgayTao = dto.NgayTao ?? DateTime.Now,
            };

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            if (dto.HinhAnhs != null && dto.HinhAnhs.Any())
            {
                foreach (var duongDan in dto.HinhAnhs)
                {
                    var image = new HinhAnhSanPham
                    {
                        MaSanPham = product.MaSua,
                        DuongDan = duongDan
                    };
                    await _imageRepository.AddAsync(image);
                }
                await _imageRepository.SaveChangesAsync();
            }

            return true;
        }

        public async Task<GetProductsAdminDTO?> GetIdProduct(int productId)
        {
            var product = await _repository.FirstOrDefaultAsync(x => x.MaSua == productId);
            if (product == null || product.Status?.ToLower() == "deleted")
                return null;

            var images = (await _imageRepository.FindAsync(x => x.MaSanPham == productId)).ToList(); // ✅ ép kiểu List
            var loai = await _loaiRepository.FirstOrDefaultAsync(x => x.MaLoai == product.MaLoai);

            return new GetProductsAdminDTO
            {
                MaSua = product.MaSua,
                TenSua = product.TenSua,
                TenLoai = loai?.TenLoai?? "Khong do",
                Gia = product.Gia,
                MoTa = product.MoTa,
                HinhAnhs = images.Select(img => img.DuongDan).ToList(), // ✅ an toàn
                NgayTao = product.NgayTao,
                Status = product.Status,
                SoLuong = product.SoLuong
            };
        }

        public async Task<bool> UpdateProduct(UpdateProductsAdminDTO updateDto)
        {
            var  re=  await _repository.FirstOrDefaultAsync(x =>  x.MaSua ==updateDto.MaSua);

            if (re == null) return false;

            re.TenSua = updateDto.TenSua;
            re.Gia = updateDto.Gia;
            re.MoTa = updateDto.MoTa;       
            re.SoLuong = updateDto.SoLuong;
            re.NgayTao = updateDto.NgayTao;
            re.Status = updateDto.Status;

            _repository.Update(re);
            await _repository.SaveChangesAsync();

            if (updateDto.HinhAnhs != null && updateDto.HinhAnhs.Any())
            {
                // ❌ XÓA ảnh cũ trước khi thêm mới
                var oldImages = await _imageRepository.FindAsync(x => x.MaSanPham == re.MaSua);
                foreach (var old in oldImages)
                {
                    _imageRepository.Delete(old);
                }

                // Thêm ảnh mới
                foreach (var duongDan in updateDto.HinhAnhs)
                {
                    var image = new HinhAnhSanPham
                    {
                        MaSanPham = re.MaSua,
                        DuongDan = duongDan
                    };
                    await _imageRepository.AddAsync(image);
                }

                await _imageRepository.SaveChangesAsync();
            }

            return true;

        }

        public async Task<bool> DeleteUser(int id)
        {
            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return false;
            }


            existingUser.Status = "deleted"; // status từ DTO


            //existingUser.MatKhau = updateDto.MatKhau; // Xóa hoặc comment dòng này

            await _repository.SaveChangesAsync();
            return true;
        }


    }
}
