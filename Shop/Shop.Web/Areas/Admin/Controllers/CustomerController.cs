using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Application.Services;

namespace Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly IAdminCustomerService _adminCustomerService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CustomerController(IAdminCustomerService adminCustomerService , IWebHostEnvironment webHostEnvironment)
        {
            _adminCustomerService = adminCustomerService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customers = await _adminCustomerService.GetAllCustomer();

            // Kiểm tra null để tránh lỗi nếu dữ liệu chưa có
            if (customers == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khách hàng nào.";
                return View(new List<GetCustomerAdminDTO>()); // Trả về danh sách rỗng
            }

            return View(customers);
        }

        public async Task<IActionResult> Detalis(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy ID người dùng.";
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách
            }

            var userDto = await _adminCustomerService.GetCustomerById(id.Value);
            if (userDto == null)
            {
                TempData["ErrorMessage"] = "Người dùng không tồn tại.";
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách
            }


            return View(userDto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy ID khách hàng.";
                return RedirectToAction("Index");
            }

            var customerDto = await _adminCustomerService.GetCustomerById(id.Value);
            if (customerDto == null)
            {
                TempData["ErrorMessage"] = "Khách hàng không tồn tại.";
                return RedirectToAction("Index");
            }

            var updateDto = new UpdateCustomerDTO
            {
                MaKh = customerDto.MaKh,
                HoTen = customerDto.HoTen,
                SoDienThoai = customerDto.SoDienThoai,
                DiaChi = customerDto.DiaChi,
                GioiTinh = customerDto.GioiTinh,
                // Ánh xạ đường dẫn ảnh hiện tại vào CurrentImagePath
                CurrentImagePath = customerDto.Image // <--- THAY ĐỔI Ở ĐÂY
                // NewImageFile sẽ là null ban đầu
            };

            return View(updateDto);
        }

        // POST: /Admin/Customer/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCustomerDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            string newImagePath = dto.CurrentImagePath; // Mặc định giữ ảnh cũ

            // Xử lý upload ảnh mới
            if (dto.NewImageFile != null)
            {
                // Thư mục lưu ảnh
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "imgCustomer");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Tên file ảnh mới: ID_MaKh_TênGốc
                string fileName = $"ID_{dto.MaKh}_{Path.GetFileName(dto.NewImageFile.FileName)}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                // Nếu có ảnh cũ thì xóa
                if (!string.IsNullOrEmpty(dto.CurrentImagePath))
                {
                    // Tách tên file từ đường dẫn
                    string oldFileName = Path.GetFileName(dto.CurrentImagePath);
                    string oldFilePath = Path.Combine(uploadsFolder, oldFileName);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath); // Xóa ảnh cũ
                    }
                }

                // Lưu ảnh mới
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.NewImageFile.CopyToAsync(stream);
                }

                // Đường dẫn lưu vào DB (dùng khi hiển thị ảnh)
                newImagePath = Path.Combine("/images/imgCustomer", fileName).Replace("\\", "/");
            }

            // Cập nhật thông tin khách hàng
            var customerToUpdate = new UpdateCustomerDTO
            {
                MaKh = dto.MaKh,
                HoTen = dto.HoTen,
                SoDienThoai = dto.SoDienThoai,
                DiaChi = dto.DiaChi,
                GioiTinh = dto.GioiTinh,
                CurrentImagePath = newImagePath
            };

            var updated = await _adminCustomerService.UpdateCustomer(customerToUpdate);

            if (updated)
            {
                TempData["SuccessMessage"] = "Cập nhật thông tin khách hàng thành công.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Cập nhật thông tin thất bại.";
                dto.CurrentImagePath = newImagePath;
                return View(dto);
            }
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(UpdateUserDTO dto)
        //{

        //    if (!ModelState.IsValid)
        //    {

        //        return View(dto);
        //    }

        //    // Gọi service để cập nhật người dùng
        //    var updated = await _adminCustomerService.UpdateUser(dto);

        //    if (updated)
        //    {
        //        TempData["SuccessMessage"] = "Cập nhật người dùng thành công.";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Cập nhật người dùng thất bại. Có thể người dùng không tồn tại.";
        //        return View(dto); // Trả về lại DTO để người dùng xem lại dữ liệu đã nhập
        //    }
        //}
    }
}
