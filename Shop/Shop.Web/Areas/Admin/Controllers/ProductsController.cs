using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;

namespace Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
   
      
    public class ProductsController : Controller
    {
        private readonly IAdminProductsService _adminCustomerService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductsController(IAdminProductsService adminCustomerService, IWebHostEnvironment webHostEnvironment)
        {
            _adminCustomerService = adminCustomerService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var customers = await _adminCustomerService.GetAllProducts();

            // Kiểm tra null để tránh lỗi nếu dữ liệu chưa có
            if (customers == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khách hàng nào.";
                return View(new List<GetCustomerAdminDTO>()); // Trả về danh sách rỗng
            }

            return View(customers);
        }
    }
}
