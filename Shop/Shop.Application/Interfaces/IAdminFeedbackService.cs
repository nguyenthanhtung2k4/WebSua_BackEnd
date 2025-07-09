using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces
{
    public interface IAdminFeedbackService
    {
        // Lấy tất cả đánh giá, bao gồm thông tin người dùng và sản phẩm liên quan
        Task<IEnumerable<GetAllFeedbackAdminDTO>> GetAllFeedbacksAsync();

        // Xóa một đánh giá dựa trên ID
        Task<bool> DeleteFeedbackAsync(int feedbackId);

        // Các phương thức khác (đã comment out trong bản gốc của bạn) có thể được thêm vào sau
        // Task<GetFeedBackAdminDTO?> GetIdProduct(int userId);
        // Task<bool> UpdateProduct(UpdateFeedBackAdminDTO updateDto);
        // Task<bool> AddProduct(AddFeedBackAdminDTO addUserDTO);
        // Task<bool> AssignRoleToUser(int userId, string roleName);
        // Task<bool> RemoveRoleFromUser(int userId, string roleName);
    }
}