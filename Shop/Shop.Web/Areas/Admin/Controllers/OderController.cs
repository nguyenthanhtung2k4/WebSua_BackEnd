using Microsoft.AspNetCore.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly IAdminOrderService _orderService;

        public OrdersController(IAdminOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            TempData[result ? "SuccessMessage" : "ErrorMessage"] = result ? "Xóa đơn hàng thành công!" : "Xóa thất bại!";
            return RedirectToAction(nameof(Index));
        }
    }

}
