using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces
{
    public interface IAdminProductsService
    {
        Task<IEnumerable<GetProductsAdminDTO>> GetAllProducts(); // Assuming UserDTO exists
        Task<GetProductsAdminDTO?> GetIdProduct(int userId);
        Task<bool> UpdateProduct(UpdateProductsAdminDTO updateDto); // Update user details/roles
        //Task<bool> DeleteUser(int userId);
        Task<bool> AddProduct(AddProductsAdminDTO addUserDTO);

        Task<bool> DeleteUser(int userId);
        //Task<bool> AssignRoleToUser(int userId, string roleName);
        //Task<bool> RemoveRoleFromUser(int userId, string roleName);

    }
}
