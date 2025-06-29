using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces
{
    public interface IAdminCustomerService
    {
        Task<IEnumerable<GetCustomerAdminDTO>> GetAllCustomer(); // Assuming UserDTO exists
        //Task<GetCustomerAdminDTO> GetCustomerById(int id);
        Task<GetCustomerAdminDTO?> GetCustomerById(int userId);

        Task<bool> UpdateCustomer(UpdateCustomerDTO updateDto); // Update user details/roles
        //Task<bool> DeleteUser(int userId);
        //Task<bool> AddCustomer(AddUserAdminDTO addUserDTO);

        //Task<bool> DeleteCustomer(int userId);
    }
}
