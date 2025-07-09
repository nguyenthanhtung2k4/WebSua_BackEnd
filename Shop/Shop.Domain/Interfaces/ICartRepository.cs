using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Infrastructure;

namespace Shop.Domain.Interfaces
{
    public interface ICartRepository: IRepository<GioHang>
    {
        Task<List<GioHang>> GetCartByCustomerAsync(int maKh);
        Task<int?> LayMaKhachHangTheoEmailAsync(string email);
    }
}
