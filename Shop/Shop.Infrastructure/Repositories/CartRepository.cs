using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Infrastructure;
using Shop.Infrastructure.Data;
using Shop.Infrastructure.Repositories;

public class CartRepository : Repository<GioHang>, ICartRepository
{
    private readonly ShopDbContext _context;

    public CartRepository(ShopDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<List<GioHang>> GetCartByCustomerAsync(int maKh)
    {
        return await _context.GioHangs
            .Include(g => g.MaSuaNavigation) // load thông tin sản phẩm
            .Where(g => g.MaKh == maKh)
            .ToListAsync();
    }
    public async Task<int?> LayMaKhachHangTheoEmailAsync(string email)
    {
        var nguoiDung = await _context.NguoiDungs.FirstOrDefaultAsync(nd => nd.Email == email);
        if (nguoiDung == null) return null;

        var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(kh => kh.MaNd == nguoiDung.MaNd);
        return khachHang?.MaKh;
    }
}
