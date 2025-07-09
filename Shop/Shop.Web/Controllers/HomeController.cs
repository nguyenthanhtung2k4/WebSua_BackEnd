using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Application.Services;
using Shop.Infrastructure;
using Shop.Web.ViewModels;

namespace Shop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _home;
        private readonly IPayService _pay;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(
                IHomeService home,
                IPayService pay,
                IWebHostEnvironment webHostEnvironment)
        {
            _home = home;
            _pay = pay;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _home.GetAllProducts();

            if (products == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm nào.";
                return View(new List<GetProductsAdminDTO>()); // Trả về danh sách rỗng
            }

            return View(products);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _home.GetIdProduct(id);
            if (product == null) return NotFound();

            var suggested = await _home.ProductSuggester(id);
            if (suggested == null) return NotFound();
            var GetIdFeedBack = await _home.GetIDFeedbackDTOs(id);

            var viewModel = new ProductDetailViewModel
            {
                Product = product,
                ProductSugGester = suggested,
                Reviews = GetIdFeedBack
            };
            return View(viewModel);
        }

        /// Them vao gio hang
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

        // Feeb Back danh gia san pham
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(FeedbackDTO model)
        {
            var maNd = HttpContext.Session.GetInt32("MaND");
            if (maNd == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để đánh giá.";
                return RedirectToAction("Detail", new { id = model.MaSua });
            }

            await _home.FeedBack(maNd, model);
            TempData["Success"] = "Cảm ơn bạn đã đánh giá!";
            return RedirectToAction("Detail", new { id = model.MaSua });
        }

        // gio Hang cart
        public async Task<IActionResult> Cart()
        {
            var maNd = HttpContext.Session.GetInt32("MaND");
            if (maNd == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để xem giỏ hàng.";
                return RedirectToAction("Index", "Auth"); // Chuyển hướng về trang đăng nhập
            }

            var carts = await _home.GetCartItemsByMaNd(maNd.Value);
            return View("Cart", carts);
        }
        
        /// Update Cart
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int maSua, string action)
        {
            var maNd = HttpContext.Session.GetInt32("MaND");

            if (maNd == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để chỉnh sửa giỏ hàng.";
                return RedirectToAction("Cart");
            }

            try
            {
                await _home.UpdateCartQuantity(maNd.Value, maSua, action);
                TempData["Success"] = "Cập nhật giỏ hàng thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
            }

            return RedirectToAction("Cart");
        }
        //  Remote Cart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int maSua)
        {
            var maNd = HttpContext.Session.GetInt32("MaND");

            if (maNd == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để xóa sản phẩm.";
                return RedirectToAction("Cart");
            }

            try
            {
                await _home.RemoveItemFromCart(maNd.Value, maSua);
                TempData["Success"] = "Xóa sản phẩm khỏi giỏ hàng thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
            }

            return RedirectToAction("Cart");
        }

        /// CheckOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            // 1. Kiểm tra đăng nhập
            var maNd = HttpContext.Session.GetInt32("MaND");
            if (maNd == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để thanh toán.";
                return RedirectToAction("Cart");
            }

            // 2. Tìm mã khách hàng tương ứng với người dùng hiện tại
            var khachHang = await _pay.GetKhachHangByMaND(maNd.Value);
            if (khachHang == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                return RedirectToAction("Cart");
            }

            int maKh = khachHang.MaKh;

            // 3. Lấy giỏ hàng
            var cartItems = await _home.GetCartItemsByMaNd(maNd.Value);
            if (cartItems == null || !cartItems.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Cart");
            }

            // 4. Tính tổng tiền
            decimal tongTien = cartItems.Sum(item => item.Gia * item.SoLuong);

            // 5. Tạo đơn hàng
            var donHang = new DonHang
            {
                MaKh = maKh,
                NgayDat = DateTime.Now,
                TongTien = tongTien,
                TrangThai = "Chờ xác nhận"
            };
            await _pay.CreateDonHangAsync(donHang);

            // 6. Tạo chi tiết đơn hàng
            foreach (var item in cartItems)
            {
                var chiTiet = new ChiTietDonHang
                {
                    MaDh = donHang.MaDh,
                    MaSua = item.MaSua,
                    SoLuong = item.SoLuong,
                    DonGia = item.Gia
                };
                await _pay.CreateChiTietDonHangAsync(chiTiet);
            }

            // 7. Xoá giỏ hàng
            await _pay.ClearCartAsync(maKh);

            TempData["Success"] = "Thanh toán thành công! Đơn hàng đã được ghi nhận.";
            return RedirectToAction("Index", "Home");
        }


        /// User
        public IActionResult User ()
        {
            return View();
        }

        // LogOut 
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Auth");
        }
    }
}