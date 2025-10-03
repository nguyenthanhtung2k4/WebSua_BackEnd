
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Infrastructure; 
using Shop.Domain.Interfaces; 

namespace Shop.Application.Services
{
    public class AdminFeedbackService : IAdminFeedbackService
    {
        private readonly IRepository<DanhGium> _feedbackRepository;
        private readonly IRepository<NguoiDung> _userRepository;
        private readonly IRepository<SanPhamSua> _productRepository;

      
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
            
            var feedbacks = await _feedbackRepository.GetAllIncludingAsync(
                dg => dg.MaNdNavigation, // Bao gồm thông tin người dùng
                dg => dg.MaSuaNavigation // Bao gồm thông tin sản phẩm
            );

         
            var feedbackDTOs = feedbacks.Select(f => new GetAllFeedbackAdminDTO
            {
                MaDg = f.MaDg,
                MaNd = f.MaNd,
                MaSua = f.MaSua,
              
                TenNguoiDung = f.MaNdNavigation?.TenDangNhap,
               
                HinhAnhNguoiDung = null, 
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