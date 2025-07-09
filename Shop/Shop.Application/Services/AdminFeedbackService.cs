// Shop.Application.Services/AdminFeedbackService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Infrastructure; // Nơi chứa các Entities của bạn (DanhGium, NguoiDung, SanPhamSua)
using Shop.Domain.Interfaces; // Nơi chứa IRepository

namespace Shop.Application.Services
{
    public class AdminFeedbackService : IAdminFeedbackService
    {
        private readonly IRepository<DanhGium> _feedbackRepository;
        private readonly IRepository<NguoiDung> _userRepository;
        private readonly IRepository<SanPhamSua> _productRepository;

        // Constructor không còn phụ thuộc vào IMapper
        public AdminFeedbackService(
            IRepository<DanhGium> feedbackRepository,
            IRepository<NguoiDung> userRepository,
            IRepository<SanPhamSua> productRepository
            )
        {
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<GetAllFeedbackAdminDTO>> GetAllFeedbacksAsync()
        {
            // Lấy tất cả đánh giá, bao gồm thông tin người dùng và sản phẩm liên quan
            // Sử dụng các navigation properties đúng tên là MaNdNavigation và MaSuaNavigation
            var feedbacks = await _feedbackRepository.GetAllIncludingAsync(
                dg => dg.MaNdNavigation, // Bao gồm thông tin người dùng
                dg => dg.MaSuaNavigation // Bao gồm thông tin sản phẩm
            );

            // *** Mapping thủ công tại đây ***
            var feedbackDTOs = feedbacks.Select(f => new GetAllFeedbackAdminDTO
            {
                MaDg = f.MaDg,
                MaNd = f.MaNd,
                MaSua = f.MaSua,
                // Lấy tên người dùng từ TenDangNhap của MaNdNavigation
                TenNguoiDung = f.MaNdNavigation?.TenDangNhap,
                // Giả định NguoiDung có thuộc tính HinhAnh. Nếu không, hãy đặt null hoặc xử lý phù hợp.
                HinhAnhNguoiDung = null, // Vì entity NguoiDung hiện tại không có thuộc tính HinhAnh.
                                         // Nếu bạn thêm HinhAnh vào NguoiDung, hãy sửa thành: f.MaNdNavigation?.HinhAnh
                                         // Lấy tên sản phẩm từ TenSua của MaSuaNavigation
                TenSanPham = f.MaSuaNavigation?.TenSua,
                // Lấy hình ảnh sản phẩm từ HinhAnh của MaSuaNavigation
                HinhAnhSanPham = f.MaSuaNavigation?.HinhAnh,
                NoiDung = f.NoiDung,
                Star = f.Star,
                NgayDanhGia = f.NgayDanhGia
            }).ToList();

            return feedbackDTOs;
        }

        public async Task<bool> DeleteFeedbackAsync(int feedbackId)
        {
            var feedbackToDelete = await _feedbackRepository.GetByIdAsync(feedbackId);

            if (feedbackToDelete == null)
            {
                return false; // Không tìm thấy đánh giá
            }

            _feedbackRepository.Delete(feedbackToDelete);
            var result = await _feedbackRepository.SaveChangesAsync(); // Lưu thay đổi vào DB

            return result > 0; // Trả về true nếu có ít nhất 1 bản ghi được xóa
        }
    }
}