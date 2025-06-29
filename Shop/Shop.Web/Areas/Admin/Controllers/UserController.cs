using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;

namespace Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        public readonly IAdminUserService _adminUserService; // Đổi tên biến để rõ ràng hơn

        public UserController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _adminUserService.GetAllUsers(); 
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy ID người dùng.";
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách
            }

            var userDto = await _adminUserService.GetUserById(id.Value);
            if (userDto == null)
            {
                TempData["ErrorMessage"] = "Người dùng không tồn tại.";
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách
            }

            
            return View(userDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateUserDTO dto)
        {
           
            if (!ModelState.IsValid)
            {
            
                return View(dto);
            }

            // Gọi service để cập nhật người dùng
            var updated = await _adminUserService.UpdateUser(dto);

            if (updated)
            {
                TempData["SuccessMessage"] = "Cập nhật người dùng thành công.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Cập nhật người dùng thất bại. Có thể người dùng không tồn tại.";
                return View(dto); // Trả về lại DTO để người dùng xem lại dữ liệu đã nhập
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View( new AddUserAdminDTO());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserAdminDTO dto)
        {

            if (!ModelState.IsValid)
            {

                return View(dto);
            }

            // Gọi service để cập nhật người dùng
            var updated = await _adminUserService.AddUser(dto);

            if (updated)
            {
                //TempData["SuccessMessage"] = "Thêm người dùng thành công !";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Thêm người dùng thất bại. Có thể người dùng  tồn tại.";
                return View(dto); // Trả về lại DTO để người dùng xem lại dữ liệu đã nhập
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // SỬA LỖI Ở ĐÂY: Thay đổi .Remove(id) thành .DeleteUser(id)
            var deleted = await _adminUserService.DeleteUser(id);

            if (deleted)
            {
                TempData["SuccessMessage"] = "Vô hiệu hóa người dùng thành công.";
            }
            else
            {
                TempData["ErrorMessage"] = "Vô hiệu hóa người dùng thất bại. Có thể người dùng không tồn tại.";
            }

            return RedirectToAction("Index");
        }

    }
}