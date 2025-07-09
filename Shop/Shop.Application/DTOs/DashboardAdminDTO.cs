using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs
{
    public class DashboardAdminDTO
    {
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalOrders { get; set; }
        public decimal RevenueToday { get; set; }
        public decimal RevenueAllTime { get; set; }
        public int TotalSoldItems { get; set; }
    }
}
