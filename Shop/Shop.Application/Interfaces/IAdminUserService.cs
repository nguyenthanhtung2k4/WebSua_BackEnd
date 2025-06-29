
using Shop.Application.DTOs;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces
{
    public interface IAdminUserService
    {
        Task<IEnumerable<GetUserAdminDTO>> GetAllUsers(); // Assuming UserDTO exists
        Task<GetUserAdminDTO> GetUserById(int userId);
        Task<bool> UpdateUser(UpdateUserDTO updateDto); // Update user details/roles
        //Task<bool> DeleteUser(int userId);
        Task<bool> AddUser(AddUserAdminDTO addUserDTO);

        Task<bool> DeleteUser(int userId);
        //Task<bool> AssignRoleToUser(int userId, string roleName);
        //Task<bool> RemoveRoleFromUser(int userId, string roleName);
    }
}
