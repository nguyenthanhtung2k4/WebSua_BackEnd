using Shop.Application.DTOs;

public interface IAdminOrderService
{
    Task<IEnumerable<GetAllOrderAdminDTO>> GetAllOrdersAsync();
    Task<bool> DeleteOrderAsync(int id);
}
