using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class AuthController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignOut()
        {
           
            HttpContext.Session.Clear(); 

            
            return RedirectToAction("Index", "Auth", new { area = "" });
        }
    }
}
