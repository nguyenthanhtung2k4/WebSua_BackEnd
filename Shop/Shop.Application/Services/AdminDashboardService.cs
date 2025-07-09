using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Infrastructure;

namespace Shop.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IRepository<SanPhamSua> _productRepo;
        private readonly IRepository<NguoiDung> _customerRepo;
        private readonly IRepository<DonHang> _orderRepo;
        private readonly IRepository<ChiTietDonHang> _orderDetailRepo;

        public AdminDashboardService(
            IRepository<SanPhamSua> productRepo,
            IRepository<NguoiDung> customerRepo,
            IRepository<DonHang> orderRepo,
            IRepository<ChiTietDonHang> orderDetailRepo
        )
        {
            _productRepo = productRepo;
            _customerRepo = customerRepo;
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
        }

        public async Task<DashboardAdminDTO> GetDashboardStatsAsync()
        {




            var products = await _productRepo.GetAllAsync();
            var customers = await _customerRepo.GetAllAsync();
            var orders = await _orderRepo.GetAllAsync();
            var orderDetails = await _orderDetailRepo.GetAllAsync();
            var today = DateTime.Today;

           
            var validProducts = products.Where(p => p?.Status?.ToLower()!= "deleted");
            var validCustomers = customers.Where(c => c?.status?.ToLower() != "deleted");
            




            return new DashboardAdminDTO
            {
                TotalProducts = products.Count(),
                TotalCustomers = customers.Count(),
                TotalOrders = orders.Count(),
                RevenueToday = orders
                    .Where(o => o.NgayDat.HasValue && o.NgayDat.Value.Date == today)
                    .Sum(o => o.TongTien ?? 0),
                RevenueAllTime = orders.Sum(o => o.TongTien ?? 0),
                TotalSoldItems = orderDetails.Sum(d => d.SoLuong ?? 0)
            };
        }
    }

}
