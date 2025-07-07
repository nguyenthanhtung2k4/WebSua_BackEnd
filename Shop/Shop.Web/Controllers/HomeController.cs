using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Application.Services;
using Shop.Web.ViewModels;

namespace Shop.Web.Controllers
{
    public class HomeController : Controller
    {
  
        private readonly IHomeService _home;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public HomeController(
                IHomeService home, 
                IWebHostEnvironment webHostEnvironment)
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


        public async Task<IActionResult> Cart()
        {
           

            return View();
        }


        public async Task<IActionResult> Detail(int id)
        {
            var product = await _home.GetIdProduct(id);
            if (product == null) return NotFound();

            var suggested = await _home.ProductSuggester(id);
            var viewModel = new ProductDetailViewModel
            {
                Product = product,
                ProductSugGester = suggested
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBag(AddToCartDTO dto)
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Vui lòng đăng nhập để thêm vào giỏ hàng.";
                return RedirectToAction("Detail", "Home", new { id = dto.MaSua });
            }

            try
            {
                await _home.AddproductToCart(email, dto);
                TempData["Success"] = "Thêm vào giỏ hàng thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
            }

            return RedirectToAction("Detail", "Home", new { id = dto.MaSua });
        }






        // LogOut 
        public IActionResult LogOut()
        {

            return View("Index", "Auth");
        }

    }
}
