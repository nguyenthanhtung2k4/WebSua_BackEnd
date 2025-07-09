using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Infrastructure;

namespace Shop.Application.Services
{
    public class PayService : IPayService
    {
        private readonly IRepository<SanPhamSua> _repository;
        private readonly IRepository<HinhAnhSanPham> _imageRepository;
        private readonly IRepository<DonHang> _donHangRepo;
        private readonly IRepository<ChiTietDonHang> _chiTietRepo;
        private readonly IRepository<GioHang> _gioHangRepo;
        private readonly IRepository<KhachHang> _khachHangRepo;

        public PayService(
            IRepository<SanPhamSua> repository,
            IRepository<HinhAnhSanPham> imageRepository,
            IRepository<DonHang> donHangRepo,
            IRepository<ChiTietDonHang> chiTietRepo,
            IRepository<GioHang> gioHangRepo,
            IRepository<KhachHang> khachHang
        )
        {
            _repository = repository;
            _imageRepository = imageRepository;
            _donHangRepo = donHangRepo;
            _chiTietRepo = chiTietRepo;
            _gioHangRepo = gioHangRepo;
            _khachHangRepo = khachHang;
        }
        public async Task<KhachHang?> GetKhachHangByMaND(int maNd)
        {
            return await _khachHangRepo.FirstOrDefaultAsync(kh => kh.MaNd == maNd);
        }
        public async Task CreateDonHangAsync(DonHang donHang)
        {
            await _donHangRepo.AddAsync(donHang);
            await _donHangRepo.SaveChangesAsync();
        }

        public async Task CreateChiTietDonHangAsync(ChiTietDonHang chiTiet)
        {
            await _chiTietRepo.AddAsync(chiTiet);
            await _chiTietRepo.SaveChangesAsync();
        }

        public async Task ClearCartAsync(int maKh)
        {
            var cartItems = await _gioHangRepo.FindAsync(x => x.MaKh == maKh);
            foreach (var item in cartItems)
            {
                _gioHangRepo.Delete(item);
            }
            await _gioHangRepo.SaveChangesAsync();
        }


    }
}
