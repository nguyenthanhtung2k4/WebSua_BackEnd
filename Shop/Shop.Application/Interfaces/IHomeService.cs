using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces
{
    public interface IHomeService
    {
        Task<IEnumerable<GetProductsAdminDTO>> GetAllProducts(); // Assuming UserDTO exists
        Task<GetProductsAdminDTO?> GetIdProduct(int userId);





    }
}
