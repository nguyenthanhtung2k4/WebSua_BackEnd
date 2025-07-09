using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Application.DTOs;
using Shop.Infrastructure;

namespace Shop.Application.Interfaces
{
    public interface IHomeService
    {
        Task<IEnumerable<GetProductsAdminDTO>> GetAllProducts(); // Assuming UserDTO exists
        Task<GetProductsAdminDTO?> GetIdProduct(int userId);
        //  SP de xuat
        Task<List<GetProductsAdminDTO>> ProductSuggester(int id);
        //  Add Cart 
        Task AddproductToCart(string email, AddToCartDTO dto);
        //Feed back
        Task FeedBack(int? maNd, FeedbackDTO model);
        //Feed back  ID
        Task<List<FeedbackDTO>> GetIDFeedbackDTOs(int id);
        //  Get all Cart 
        Task<List<GetAllCartDTO>> GetCartItemsByMaNd(int maNd);
        Task RemoveItemFromCart(int maNd, int maSua);
        /// Update Cart
        Task UpdateCartQuantity(int maNd, int maSua, string action);

       



    }
}
