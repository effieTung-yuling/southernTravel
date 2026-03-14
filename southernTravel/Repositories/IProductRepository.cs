using southernTravel.Model;

namespace southernTravel.Repositories
{
    public interface IProductRepository
    {
        // 取得所有會員
        Task<List<Product>> GetAllAsync();
    }
}
