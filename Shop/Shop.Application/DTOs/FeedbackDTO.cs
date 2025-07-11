﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs
{

    public class FeedbackDTO
    {

        public int MaDg { get; set; }

        public int? MaNd { get; set; }

        public int? MaSua { get; set; }
        public string? TenNguoiDung { get; set; }
        public string? HinhAnh { get; set; }

        public string? NoiDung { get; set; }

        public int? Star { get; set; }

        public DateTime? NgayDanhGia { get; set; }
    }
}
