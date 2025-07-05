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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddProductsAdminDTO dto)
        {
            if (!ModelState.IsValid) //  check  form 
            {
                return View(dto);
            }

            var imagePaths = new List<string>();

            if (dto.NewImageFiles != null && dto.NewImageFiles.Any() && dto.MaLoai.HasValue)
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "imgProducts");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                foreach (var file in dto.NewImageFiles)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string fileName = $"{Guid.NewGuid()}_SanPham_ID{dto.MaLoai}{extension}";
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    string relativePath = $"/images/imgProducts/{fileName}".Replace("\\", "/");
                    imagePaths.Add(relativePath);
                }

                dto.HinhAnhs = imagePaths;
            }

            var result = await _adminCustomerService.AddProduct(dto);

            if (result)
            {
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công.";
                return RedirectToAction("Index");
            }

            // ❌ Nếu thêm thất bại → xóa ảnh vừa lưu để tránh rác
            if (imagePaths.Any())
            {
                foreach (var path in imagePaths)
                {
                    string absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, path.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(absolutePath))
                    {
                        System.IO.File.Delete(absolutePath);
                    }
                }
            }

            TempData["ErrorMessage"] = "Thêm sản phẩm thất bại! Sản phẩm đã tồn tại hoặc có lỗi.";
            return View(dto);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy ID người dùng.";
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách
            }

            var userDto = await _adminCustomerService.GetIdProduct(id.Value);
            if (userDto == null)
            {
                TempData["ErrorMessage"] = "Người dùng không tồn tại.";
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách
            }


            return View(userDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _adminCustomerService.GetIdProduct(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm.";
                return RedirectToAction("Index");
            }

            var dto = new UpdateProductsAdminDTO
            {
                MaSua = product.MaSua,
                TenSua = product.TenSua,
                Gia = product.Gia,
                MoTa = product.MoTa,
                SoLuong = product.SoLuong,
                NgayTao = product.NgayTao,
                Status = product.Status,
                HinhAnhs = product.HinhAnhs
                // Gán thêm các trường nếu cần
            };

            return View(dto); // Trả về DTO cho View Edit
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProductsAdminDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var imagePaths = new List<string>();

            if (dto.NewImageFiles != null && dto.NewImageFiles.Any())
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "imgProducts");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                foreach (var file in dto.NewImageFiles)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string fileName = $"{Guid.NewGuid()}_EditSanPham_ID{dto.MaSua}{extension}";
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    string relativePath = $"/images/imgProducts/{fileName}".Replace("\\", "/");
                    imagePaths.Add(relativePath);
                }

                dto.HinhAnhs = imagePaths;
            }

            var result = await _adminCustomerService.UpdateProduct(dto);
            if (result)
            {
                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Cập nhật sản phẩm thất bại.";
            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // SỬA LỖI Ở ĐÂY: Thay đổi .Remove(id) thành .DeleteUser(id)
            var deleted = await _adminCustomerService.DeleteUser(id);

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

