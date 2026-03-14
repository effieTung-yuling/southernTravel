using southernTravel.Model;
using southernTravel.Repositories;
using southernTravel.DTOs;

namespace southernTravel.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // 取得所有商品
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var data = await _productRepository.GetAllProductsAsync();

            return data.Select(x => new ProductDto
            {
                ProductId = x.ProductId,
                Title = x.Title,
                Category = x.Category,
                Description = x.Description,
            }).ToList();
        }
        // 依據ID取得單一商品
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var x = await _productRepository.GetProductByIdAsync(id);

            if (x == null) return null;

            return new ProductDto
            {
                ProductId = x.ProductId,
                Title = x.Title,
                Category = x.Category,
                Description = x.Description
            };
        }
    }
}
