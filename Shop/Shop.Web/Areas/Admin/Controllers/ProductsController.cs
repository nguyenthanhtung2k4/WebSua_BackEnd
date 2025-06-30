using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Application.Services;
using Microsoft.AspNetCore.Hosting;

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


        [HttpGet]
        public IActionResult Create()
        {
            return View(new AddProductsAdminDTO());
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(AddProductsAdminDTO dto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(dto);
        //    }

        //    // Xử lý ảnh nếu có
        //    if (dto.NewImageFile != null && dto.MaLoai.HasValue)
        //    {
        //        // Tạo đường dẫn thư mục lưu ảnh
        //        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "imgProducts");
        //        if (!Directory.Exists(uploadFolder))
        //            Directory.CreateDirectory(uploadFolder);

        //        // Tạo tên file: TenSua_MaLoai.jpg
        //        string extension = Path.GetExtension(dto.NewImageFile.FileName);
        //        string fileName = $"{Guid.NewGuid()}_SanPham_{dto.MaLoai}{extension}";
        //        string filePath = Path.Combine(uploadFolder, fileName);
        //        // Nếu có ảnh cũ thì xóa
        //        if (!string.IsNullOrEmpty(dto.))
        //        {
        //            // Tách tên file từ đường dẫn
        //            string oldFileName = Path.GetFileName(dto.HinhAnh);
        //            string oldFilePath = Path.Combine(uploadsFolder, oldFileName);
        //            if (System.IO.File.Exists(oldFilePath))
        //            {
        //                System.IO.File.Delete(oldFilePath); // Xóa ảnh cũ
        //            }
        //        }
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await dto.NewImageFile.CopyToAsync(stream);
        //        }

        //        dto.HinhAnh = $"/images/imgProducts/{fileName}".Replace("\\", "/");
        //    }

        //    var result = await _adminCustomerService.AddProduct(dto);
        //    if (result)
        //    {
        //        TempData["SuccessMessage"] = "Thêm sản phẩm thành công.";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Tên sản phẩm đã tồn tại hoặc lỗi hệ thống.";
        //        return View(dto);
        //    }
        //}
    } 
}
