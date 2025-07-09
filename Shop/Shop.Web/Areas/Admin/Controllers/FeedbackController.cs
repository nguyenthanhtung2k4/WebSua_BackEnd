using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Application.Services;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class FeedbackController : Controller
    {

        private readonly IAdminFeedbackService _adminFeedbackService;
        public FeedbackController(IAdminFeedbackService adminFeedbackService)
        {
            _adminFeedbackService =    adminFeedbackService
;
        }

        [Area("Admin")]

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var feed = await _adminFeedbackService.GetAllFeedbacksAsync();

            if (feed == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đánh giá nào.";
                return View(new List<GetAllFeedbackAdminDTO>()); // ✅ Sửa đúng kiểu
            }

            return View(feed);
        }





        /// <summary>
        /// Xóa một đánh giá sản phẩm dựa trên ID.
        /// </summary>
        /// <param name="id">Mã đánh giá cần xóa.</param>
        /// <returns>Chuyển hướng về trang danh sách đánh giá.</returns>
        [HttpPost] // Chỉ rõ đây là một action HTTP POST (thường dùng cho các thao tác thay đổi dữ liệu)
        [ValidateAntiForgeryToken] // Bảo vệ chống lại tấn công CSRF
        public async Task<IActionResult> Delete(int id)
        {
            // Gọi service để thực hiện xóa đánh giá
            bool success = await _adminFeedbackService.DeleteFeedbackAsync(id);

            if (success)
            {
                // Thêm thông báo thành công vào TempData để hiển thị trên view sau khi redirect
                TempData["SuccessMessage"] = "Đánh giá đã được xóa thành công.";
            }
            else
            {
                // Thêm thông báo lỗi nếu không xóa được hoặc không tìm thấy đánh giá
                TempData["ErrorMessage"] = "Không thể xóa đánh giá hoặc không tìm thấy đánh giá với ID này.";
            }

            // Chuyển hướng về action Index để hiển thị danh sách cập nhật
            return RedirectToAction(nameof(Index));
        }
    }
}
