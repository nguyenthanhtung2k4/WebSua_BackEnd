using Shop.Application.DTOs;
using Shop.Domain.Interfaces;
using Shop.Infrastructure;

public class AdminOrderService : IAdminOrderService
{
    private readonly IRepository<DonHang> _orderRepo;
    private readonly IRepository<KhachHang> _customerRepo;

    public AdminOrderService(IRepository<DonHang> orderRepo, IRepository<KhachHang> customerRepo)
    {
        _orderRepo = orderRepo;
        _customerRepo = customerRepo;
    }

    public async Task<IEnumerable<GetAllOrderAdminDTO>> GetAllOrdersAsync()
    {
        var orders = await _orderRepo.GetAllIncludingAsync(dh => dh.MaKhNavigation);

        return orders.Select(d => new GetAllOrderAdminDTO
        {
            MaDh = d.MaDh,
            TenKhachHang = d.MaKhNavigation?.HoTen ?? "Không rõ",
            NgayDat = d.NgayDat,
            TongTien = d.TongTien,
            TrangThai = d.TrangThai
        }).ToList();
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        if (order == null) return false;

        _orderRepo.Delete(order);
        return await _orderRepo.SaveChangesAsync() > 0;
    }
}
