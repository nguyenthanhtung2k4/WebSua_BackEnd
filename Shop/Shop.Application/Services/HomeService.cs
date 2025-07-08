using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        private readonly IRepository<DanhGium> _FeedBackRepository;
        // gio hang
        private readonly ICartRepository    _gioHangRepo;


        public HomeService(
                IRepository<SanPhamSua> repository,
                IRepository<HinhAnhSanPham> imageRepository,
                IRepository<LoaiSua> loaiRepository,
                ICartRepository gioHangRepo)
        {
            _repository = repository;
            _imageRepository = imageRepository;
            _loaiRepository = loaiRepository;
            _gioHangRepo = gioHangRepo;
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
                    TenLoai = idLoai,
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

        //  San pham de  xuat
        public async Task<List<GetProductsAdminDTO>> ProductSuggester(int id)
        {
            var product = await _repository.FirstOrDefaultAsync(x => x.MaSua == id);
            if (product == null || product.Status?.ToLower() == "deleted") return new();

            var allImages = await _imageRepository.GetAllAsync();
            var goiY = (await _repository.FindAsync(x => x.MaLoai == product.MaLoai && x.MaSua != id && x.Status != "deleted"))
                        .Take(4)
                        .Select(x => new GetProductsAdminDTO
                        {
                            MaSua = x.MaSua,
                            TenSua = x.TenSua,  
                            Gia = x.Gia,
                            SoLuong = x.SoLuong,
                            MoTa = x.MoTa,
                            Status = x.Status,
                            NgayTao = x.NgayTao,
                            HinhAnhs = allImages.Where(i => i.MaSanPham == x.MaSua).Select(i => i.DuongDan).ToList()
                        }).ToList();
            return goiY;
        }


        public async Task AddproductToCart(string email, AddToCartDTO dto)
        {
            var maKh = await _gioHangRepo.LayMaKhachHangTheoEmailAsync(email);
            if (maKh == null)
                throw new Exception("Không tìm thấy khách hàng.");

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            var existingItem = await _gioHangRepo.FirstOrDefaultAsync(x => x.MaKh == maKh && x.MaSua == dto.MaSua);

            if (existingItem != null)
            {
                // Nếu đã tồn tại, cập nhật số lượng
                existingItem.SoLuong += dto.SoLuong;
                existingItem.NgayTao = DateTime.Now;
                _gioHangRepo.Update(existingItem);
            }
            else
            {
                // Nếu chưa có, thêm mới
                var gioHang = new GioHang
                {
                    MaKh = maKh,
                    MaSua = dto.MaSua,
                    SoLuong = dto.SoLuong,
                    NgayTao = DateTime.Now
                };
                await _gioHangRepo.AddAsync(gioHang);
            }

            await _gioHangRepo.SaveChangesAsync();
        }

        ////  Feeed back
        public async  Task FeedBack(int? maNd, FeedbackDTO model)
        {
          
            
            var review = new DanhGium
            {
                MaNd = maNd,
                MaSua = model.MaSua,
                NoiDung = model.NoiDung,
                Star = model.Star,
                NgayDanhGia = DateTime.Now
            };

            await _FeedBackRepository.AddAsync(review);
            await _FeedBackRepository.SaveChangesAsync();
        }


    }
}
