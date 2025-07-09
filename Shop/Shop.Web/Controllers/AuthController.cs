using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Application.Services;

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
            return View(new RegisterDTOs()); 
        }

        /// LOGIN
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {


            var loginResult = await _nguoiDungService.Login(email, password);

            if (loginResult.Success)
            {
                if (loginResult.Role == "Admin")
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                ViewBag.EmailValue = email;
                TempData["LoginError"] = "Email hoặc mật khẩu không đúng.";
                return View("Index" , new RegisterDTOs() );
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

        [HttpPost] 
        [ValidateAntiForgeryToken] 
        public IActionResult SignOut()
        {
           
           
            HttpContext.Session.Clear();

           
            return RedirectToAction("Index", "Auth");
        }
    }
}
