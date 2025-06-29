using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;

namespace Shop.Web.Controllers
{
    public class TestController : Controller
    {
         public readonly IAdminUserService _context;
        public TestController(IAdminUserService context)
        {
            _context = context;
        }




        [HttpGet]
        // GET: UserController
        public async Task<IActionResult> Index()
        {

            var user = await _context.GetAllUsers();
            return View(user);
        }
        [HttpPost]
        // GET: UserController/Details/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.GetUserById(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}
