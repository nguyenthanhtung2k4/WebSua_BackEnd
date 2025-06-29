using System;
using System.Collections.Generic;

namespace Shop.Application.DTOs
{
    public class GetCustomerAdminDTO
    {
        public int MaKh { get; set; }

        public int MaNd { get; set; }

        public string? HoTen { get; set; }

        public string? SoDienThoai { get; set; }

        public string? DiaChi { get; set; }
        public string? GioiTinh { get; set; }
        public string? Image { get; set; }


       
        // DTO con
        public GetUserAdminDTO? NguoiDungs { get; set; }
       
    }
}
