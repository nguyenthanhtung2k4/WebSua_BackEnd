using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Infrastructure;

namespace Shop.Application.Interfaces
{
    public interface IPayService
    {
        Task CreateDonHangAsync(DonHang donHang);
        Task CreateChiTietDonHangAsync(ChiTietDonHang chiTiet);
        Task ClearCartAsync(int maKh);
        Task<KhachHang?> GetKhachHangByMaND(int maNd);

    }
}
