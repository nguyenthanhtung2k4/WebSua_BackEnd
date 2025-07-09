using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs
{
    public class LoginResultDTO
    {
        public bool Success { get; set; }
        public string? Role { get; set; }
    }
}
