using southernTravel.DTOs;
using southernTravel.Model;

namespace southernTravel.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();

        Task<ProductDto?> GetProductByIdAsync(int id);
    }
}
