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
        private readonly IRepository<NguoiDung> _NguoidungRepository;
        private readonly IRepository<GioHang> _giohangRepository;
        private readonly IRepository<KhachHang> _khachHangRepository;
        // gio hang
        private readonly ICartRepository _gioHangRepo;


        public HomeService(
                IRepository<SanPhamSua> repository,
                IRepository<HinhAnhSanPham> imageRepository,
                IRepository<LoaiSua> loaiRepository,
                ICartRepository gioHangRepo,
                IRepository<DanhGium> feedback,
                IRepository<NguoiDung> nguoiDung,
                IRepository<KhachHang> khachHangRepository,
                IRepository<GioHang> gioHangRepository
            )
        {
            _repository = repository;
            _imageRepository = imageRepository;
            _loaiRepository = loaiRepository;
            _gioHangRepo = gioHangRepo;
            _FeedBackRepository = feedback;
            _NguoidungRepository = nguoiDung;
            _khachHangRepository = khachHangRepository;
            _giohangRepository = gioHangRepository;
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
        public async Task FeedBack(int? maNd, FeedbackDTO model)
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
        ////  Feeed Id 
        public async Task<List<FeedbackDTO>> GetIDFeedbackDTOs(int maSua)
        {
            var danhGiaList = await _FeedBackRepository
                .GetAllIncludingAsync(dg => dg.MaNdNavigation);

            var danhGiaTheoSanPham = danhGiaList
                .Where(dg => dg.MaSua == maSua)
                .Select(dg => new FeedbackDTO
                {
                    MaNd = dg.MaNd,
                    MaSua = dg.MaSua,
                    NoiDung = dg.NoiDung,
                    Star = dg.Star,
                    NgayDanhGia = dg.NgayDanhGia,
                    TenNguoiDung = dg.MaNdNavigation?.TenDangNhap // Giả sử lấy từ bảng NguoiDung
                })
                .ToList();

            return danhGiaTheoSanPham;
        }


        /// <summary>
        /// Lấy danh sách giỏ hàng theo MaND (người dùng)
        /// </summary>
        public async Task<List<GetAllCartDTO>> GetCartItemsByMaNd(int maNd)
        {
            var khachHang = await _khachHangRepository.FirstOrDefaultAsync(kh => kh.MaNd == maNd);

            if (khachHang == null)
                return new List<GetAllCartDTO>();

            var gioHangs = await _giohangRepository.GetAllIncludingAsync(x => x.MaSuaNavigation);
            var allImages = await _imageRepository.GetAllAsync(); 

            var carts = gioHangs
                .Where(gh => gh.MaKh == khachHang.MaKh)
                .Select(gh =>
                {
                    var hinhAnh = allImages
                        .Where(img => img.MaSanPham == gh.MaSua)
                        .Select(img => img.DuongDan)
                        .FirstOrDefault();
                    return new GetAllCartDTO
                    {
                        MaGh = gh.MaGh,
                        MaSua = gh.MaSua ?? 0,
                        TenSua = gh.MaSuaNavigation?.TenSua,
                        HinhAnh = hinhAnh ?? "/images/no-image.png", 
                        Gia = gh.MaSuaNavigation?.Gia ?? 0,
                        SoLuong = gh.SoLuong ?? 1,
                        NgayTao = gh.NgayTao
                    };
                })
                .ToList();

            return carts;
        }


        public async Task RemoveItemFromCart(int maNd, int maSua)
        {
            var khachHang = await _khachHangRepository
                .FirstOrDefaultAsync(kh => kh.MaNd == maNd);

            if (khachHang == null) return;

            var item = (await _giohangRepository
                .FindAsync(x => x.MaKh == khachHang.MaKh && x.MaSua == maSua))
                .FirstOrDefault();

            if (item != null)
            {
                _giohangRepository.Delete(item);
                await _giohangRepository.SaveChangesAsync();
            }
        }


        /// Update  Cart
        public async Task UpdateCartQuantity(int maNd, int maSua, string action)
        {
            var khachHang = await _khachHangRepository.FirstOrDefaultAsync(k => k.MaNd == maNd);
            if (khachHang == null) throw new Exception("Không tìm thấy khách hàng.");

            var gioHang = await _giohangRepository.FirstOrDefaultAsync(g => g.MaKh == khachHang.MaKh && g.MaSua == maSua);
            if (gioHang == null) throw new Exception("Không tìm thấy sản phẩm trong giỏ hàng.");

            if (action == "increase")
            {
                gioHang.SoLuong += 1;
            }
            else if (action == "decrease")
            {
                if (gioHang.SoLuong > 1)
                    gioHang.SoLuong -= 1;
                else
                    throw new Exception("Không thể giảm thấp hơn 1.");
            }

            gioHang.NgayTao = DateTime.Now;
            _giohangRepository.Update(gioHang);
            await _giohangRepository.SaveChangesAsync();
        }




    }
}
