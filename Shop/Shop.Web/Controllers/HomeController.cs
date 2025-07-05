using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Application.Services;

namespace Shop.Web.Controllers
{
    public class HomeController : Controller
    {
  
            private readonly IHomeService _home;
            private readonly IWebHostEnvironment _webHostEnvironment;


            public HomeController(IHomeService home, IWebHostEnvironment webHostEnvironment)
            {
                _home = home;
                _webHostEnvironment = webHostEnvironment;
            }
            public async Task<IActionResult> Index()
            {
                var customers = await _home.GetAllProducts();

                // Kiểm tra null để tránh lỗi nếu dữ liệu chưa có
                if (customers == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy khách hàng nào.";
                    return View(new List<GetCustomerAdminDTO>()); // Trả về danh sách rỗng
                }

                return View(customers);
            }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy ID người dùng.";
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách
            }

            var userDto = await _home.GetIdProduct(id.Value);
            if (userDto == null)
            {
                TempData["ErrorMessage"] = "Người dùng không tồn tại.";
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách
            }


            return View(userDto);
        }





    }
}
