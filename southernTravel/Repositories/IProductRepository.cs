using southernTravel.Model;

namespace southernTravel.Repositories
{
    public interface IProductRepository
    {
        // 取得所有產品
        Task<List<Product>> GetAllProductsAsync();
        // 依照ID取得產品
        Task<Product?> GetProductByIdAsync(int id);

        Task<Product> CreateProductAsync(Product product);

        Task UpdateProductAsync(Product product);

        Task DeleteProductAsync(Product product);
    }
}
