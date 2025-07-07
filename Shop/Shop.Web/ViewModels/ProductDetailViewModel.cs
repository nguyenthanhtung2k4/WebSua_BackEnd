using Shop.Application.DTOs;

namespace Shop.Web.ViewModels
{
    public class ProductDetailViewModel
    {

        public GetProductsAdminDTO? Product { get; set; } 
        public List<GetProductsAdminDTO> ProductSugGester { get; set; } = new();

    }
}
