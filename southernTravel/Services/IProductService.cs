using southernTravel.Model;

namespace southernTravel.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
    }
}
