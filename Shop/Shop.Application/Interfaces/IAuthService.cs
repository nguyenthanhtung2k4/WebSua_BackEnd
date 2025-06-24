using Shop.Application.DTOs;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Login(string username, string password);
        Task<bool> Register(RegisterDTOs dtos);
    }
}
