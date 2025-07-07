using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;

namespace Shop.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _nguoiDungService;

        public AuthController(IAuthService nguoiDungService)
        {
            _nguoiDungService = nguoiDungService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(); 
        }

        /// LOGIN
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _nguoiDungService.Login(email, password);

            if (result)
            {
                // Luu session 
                HttpContext.Session.SetString("Email", email);
                HttpContext.Session.SetString("Pass", password);

                TempData["Success"] = "Đăng nhập thành công!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
                return View("Index");
            }
        }


        /// Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTOs dtos,string comfirmPass)
        {
            if (dtos.MatKhau != comfirmPass)
            {
                ViewBag.Error_3 = "Mật khẩu và xác nhận mật khẩu không khớp.";
                return View("Index", dtos); // Quay lại view kèm dữ liệu
            }
            if (dtos.MatKhau != comfirmPass ||  dtos.MatKhau.Length <= 6 )
            {
                ViewBag.Error_3 = "Đặt Mật khẩu lớn hơn 6 kí tự ! ";
                return View("Index", dtos); // Quay lại view kèm dữ liệu
            }
            var result = await _nguoiDungService.Register(dtos);

            if (result)
            {
                TempData["Success"] = "Đăng ký thành công, mời bạn đăng nhập!";
                return RedirectToAction("Index","Auth");
            }
            else
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                return View("Index");
            }
        }
    }
}
