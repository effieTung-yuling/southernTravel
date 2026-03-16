using southernTravel.DTOs;

namespace southernTravel.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();

        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);

        Task<bool> UpdateProductAsync(int id, UpdateProductDto dto);

        Task<bool> DeleteProductAsync(int id);
    }
}
